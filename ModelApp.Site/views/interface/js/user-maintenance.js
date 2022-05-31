UserMaintenance = {

    self: this

    , init: function () {
        self.$loading = document.querySelector('.loading');
        self.$frmUserMaintenance = document.getElementById('frmUserMaintenance');
        self.$txtName = document.getElementById('txtName');
        self.$selUserRole = document.getElementById('selUserRole');
        self.$txtEmail = document.getElementById('txtEmail');
        self.$txtSecretPhrase = document.getElementById('txtSecretPhrase');
        self.$btnResetPassword = document.getElementById('btnResetPassword');
        self.$txtPassword = document.getElementById('txtPassword');
        self.$txtConfirmPassword = document.getElementById('txtConfirmPassword');
        self.$chkActive = document.getElementById('chkActive');
        self.$chkActive.checked = true;
        self.$fupAvatar = document.getElementById('fupAvatar');
        self.$imgAvatar = document.getElementById('imgAvatar');
        self.$btnRemoveImage = document.getElementById('btnRemoveImage');
        self.$btnSave = document.getElementById('btnSave');
        self.$btnCancel = document.getElementById('btnCancel');

        this.attachEvent();

        self.$UserID = sessionStorage.getItem('UserID');
        self.$user = null;
        this.loadDataUserRole();
        self.$userRole = null;
        self.$userRoles = [];
        self.$avatar = '';

        if (Number(self.$UserID) != 0) {
            this.load(self.$UserID);
        } else {
            self.$btnResetPassword.classList.add('w3-hide');
            UserMaintenance.showForm();
        }
    }

    , attachEvent: function () {
        self.$fupAvatar.addEventListener('change', function () {
            if (self.$fupAvatar.files.length > 0) {
                if (!UserMaintenance.checkImageExtension(self.$fupAvatar)) {
                    self.$fupAvatar.value = '';
                    Dialog.show('Aviso!', 'O arquivo não é uma imagem.', Dialog.MessageType.WARNING, Dialog.DisplayType.MODAL);
                    return;
                }
                UserMaintenance.imageToBase64(self.$fupAvatar);
            }
        });

        self.$btnRemoveImage.addEventListener('click', function () {
            UserMaintenance.removeAvatar();
        });

        self.$btnSave.addEventListener('click', function () {
            UserMaintenance.save();
        });

        self.$btnCancel.addEventListener('click', function () {
            UserMaintenance.cancel();
        });

        self.$selUserRole.addEventListener('change', function () {
            var UserRoleID = self.$selUserRole.options[self.$selUserRole.selectedIndex].value;
            for (var i = 0; i < self.$userRoles.length; i++) {
                if (UserRoleID == self.$userRoles[i].UserRoleID) {
                    self.$userRole = self.$userRoles[i];
                    break;
                }
            }
        });

        self.$btnResetPassword.addEventListener('click', function () {
            UserMaintenance.resetPassword();
        });
    }

    , clear: function () {
        self.$txtName.value = '';
        self.$selUserRole.options[0].selected = true;
        self.$txtEmail.value = '';
        self.$txtSecretPhrase.value = '';
        self.$txtPassword.value = '';
        self.$txtConfirmPassword.value = '';
        self.$chkActive.checked = true;
    }

    , check: function () {
        var fields = [];
        var message = '';

        if (self.$txtName.value.length == 0)
            fields.push('Nome');

        if (self.$selUserRole.options[self.$selUserRole.selectedIndex].value == 0)
            fields.push('Papel de usuário');

        if (self.$txtEmail.value.length == 0)
            fields.push('E-mail');

        if (self.$txtPassword.value.length == 0)
            fields.push('Senha');

        if (self.$txtConfirmPassword.value.length == 0)
            fields.push('Confirmar senha');
        
        for (var i = 0; i < fields.length; i++) {
            if (fields.length == 1) {
                message += 'Informe o seguinte campo: ' + fields[i];
            } else if (fields.length > 1) {
                if (i == 0) {
                    message += 'Informe os seguintes campos: ';
                }
                if (i == (fields.length - 1)) {
                    message += ' e ' + fields[i];
                } else if (i == (fields.length - 2)) {
                    message += fields[i] + ' ';
                } else {
                    message += fields[i] + ', ';
                }
            }
        }

        if (fields.length != 0) {
            message += '.';
        }

        return message;
    }

    , save: function () {
        try {

            var check = this.check();

            if (check.length > 0) {
                Dialog.show('Aviso!', check, Dialog.MessageType.WARNING, Dialog.DisplayType.MODAL);
                return;
            }

            if (self.$txtPassword.value != self.$txtConfirmPassword.value) {
                Dialog.show('Aviso!', 'Senha de confirmação diferente.', Dialog.MessageType.WARNING, Dialog.DisplayType.MODAL);
                return;
            }

            self.$btnSave.disabled = true;

            var xhttp;
            var op = '';

            if (window.XMLHttpRequest) {
                xhttp = new XMLHttpRequest();
            } else {
                xhttp = new ActiveXObject('Microsoft.XMLHTTP');
            }

            xhttp.onreadystatechange = function () {
                if (this.readyState === 4 && (this.status === 200 || this.status === 201 || this.status === 204)) {
                    self.$btnSave.disabled = false;
                    if (Number(self.$UserID) == 0) {
                        UserMaintenance.clear();
                    }
                    Dialog.show('Sucesso!', 'Registro ' + op, Dialog.MessageType.SUCCESS, Dialog.DisplayType.MODAL);
                }
            };

            if (Number(self.$UserID) != 0) {

                var currentUser = {
                    UserID: Number(self.$UserID)
                    , Name: self.$txtName.value
                    , Email: self.$txtEmail.value
                    , SecretPhrase: self.$txtSecretPhrase.value
                    , Password: self.$txtPassword.value
                    , Active: self.$chkActive.checked
                    , UserRoleID: self.$userRole.UserRoleID
                    , UserRole: {
                        UserRoleID: self.$userRole.UserRoleID
                        , Name: self.$userRole.Name
                        , Description: self.$userRole.Description
                        , CreatedBy: self.$userRole.CreatedBy
                        , CreatedIn: self.$userRole.CreatedIn
                        , UpdateBy: self.$userRole.UpdateBy
                        , UpdatedIn: self.$userRole.UpdatedIn
                    }
                    , Avatar: self.$avatar
                    , CreatedBy: self.$user.CreatedBy
                    , CreatedIn: self.$user.CreatedIn
                    , UpdatedBy: JSON.parse(sessionStorage.getItem('loginUser')).Name
                    , UpdatedIn: new Date()
                };

                op = 'atualizado.';
                data = JSON.stringify(currentUser);
                xhttp.open('put', Server.setService('api/users/' + self.$UserID), true);
                xhttp.setRequestHeader('Content-type', 'application/json; charset=utf-8');
                xhttp.send(data);

            } else {

                var newUser = {
                    UserID: Number(self.$UserID)
                    , Name: self.$txtName.value
                    , Email: self.$txtEmail.value
                    , SecretPhrase: self.$txtSecretPhrase.value
                    , Password: self.$txtPassword.value
                    , Active: self.$chkActive.checked
                    , UserRoleID: self.$userRole.UserRoleID
                    , UserRole: {
                        UserRoleID: self.$userRole.UserRoleID
                        , Name: self.$userRole.Name
                        , Description: self.$userRole.Description
                        , CreatedBy: self.$userRole.CreatedBy
                        , CreatedIn: self.$userRole.CreatedIn
                        , UpdateBy: self.$userRole.UpdateBy
                        , UpdatedIn: self.$userRole.UpdatedIn
                    }
                    , Avatar: self.$avatar
                    , CreatedBy: JSON.parse(sessionStorage.getItem('loginUser')).Name
                    , CreatedIn: new Date()
                };

                op = 'salvo.';
                data = JSON.stringify(newUser);
                xhttp.open('post', Server.setService('api/users'), true);
                xhttp.setRequestHeader('Content-type', 'application/json; charset=utf-8');
                xhttp.send(data);
            }

        } catch (e) {
            self.$btnSave.disabled = false;
            Dialog.show
                (
                    'Ops!'
                    , 'Ocorreu um problema. Mensagem do sistema: ' + e.message + '. Stacktrace: ' + e.stacktrace
                    , Dialog.MessageType.ERROR
                    , Dialog.DisplayType.MODAL
                );
        }
    }

    , loadDataUserRole: function () {
        var xhttp;

        if (window.XMLHttpRequest) {
            xhttp = new XMLHttpRequest();
        } else {
            xhttp = new ActiveXObject('Microsoft.XMLHTTP');
        }

        xhttp.onreadystatechange = function () {
            if (this.readyState === 4 && this.status === 200) {
                UserMaintenance.populeUserRole(JSON.parse(this.responseText));
            }
        };

        xhttp.open('get', Server.setService('api/userroles'), true);
        xhttp.send();
    }

    , populeUserRole: function (dataSource) {
        self.$selUserRole.innerHTML = '';

        var opSel = document.createElement('option');
        opSel.value = '0';
        opSel.textContent = 'Selecione o papel';
        self.$selUserRole.appendChild(opSel);

        for (var i = 0; i < dataSource.length; i++) {
            self.$userRoles.push(dataSource[i]);
            var op = document.createElement('option');
            op.value = dataSource[i].UserRoleID;
            op.textContent = dataSource[i].Name;
            self.$selUserRole.appendChild(op);
        }
    }

    , load: function (UserID) {
        var xhttp;

        if (window.XMLHttpRequest) {
            xhttp = new XMLHttpRequest();
        } else {
            xhttp = new ActiveXObject('Microsoft.XMLHTTP');
        }

        xhttp.onreadystatechange = function () {
            if (this.readyState === 4 && this.status === 200) {
                self.$user = JSON.parse(this.responseText);
                self.$txtName.value = self.$user.Name;
                self.$txtEmail.value = self.$user.Email;
                self.$txtSecretPhrase.value = self.$user.SecretPhrase;
                self.$txtPassword.value = self.$user.Password;
                self.$txtConfirmPassword.value = self.$user.Password;
                self.$chkActive.checked = self.$user.Active;

                if (self.$user.Avatar != null || self.$user.Avatar != undefined) {
                    if (self.$user.Avatar.length > 0) {
                        self.$imgAvatar.setAttribute('src', 'data:image/png;base64,' + self.$user.Avatar);
                    }
                }

                // Keep the option selected by UserRoleID.
                for (var i = 0; i < self.$selUserRole.options.length; i++) {
                    if (Number(self.$selUserRole.options[i].value) === self.$user.UserRole.UserRoleID) {
                        self.$selUserRole.options[i].defaultSelected = true;
                        break;
                    }
                }

                // Keep the data object of the dataSource by UserRoleID.
                for (var x = 0; x < self.$userRoles.length; x++) {
                    if (Number(self.$userRoles[x].UserRoleID) === self.$user.UserRole.UserRoleID) {
                        self.$userRole = self.$userRoles[x];
                        break;
                    }
                }
                UserMaintenance.showForm();
            }
        };

        xhttp.open('get', Server.setService('api/users/' + UserID), true);
        xhttp.send();
    }

    , cancel: function () {
        Utils.loadPage('user-listing.html');
    }

    , showForm: function () {
        self.$loading.classList.add('w3-hide');
        self.$frmUserMaintenance.classList.remove('w3-hide');
    }

    , checkImageExtension: function (el) {
        var isImage = false;
        var fileExtension = el.files[0].name.split('.').pop();

        switch (fileExtension.toLowerCase()) {
            case 'bmp':
            case 'jpg':
            case 'jpeg':
            case 'gif':
            case 'png':
                isImage = true;
                break;
            default:
                break;
        }

        return isImage;
    }

    , imageToBase64: function (el) {
        var file = el.files[0];
        var reader = new FileReader();
        reader.onloadend = function () {
            self.$imgAvatar.setAttribute('src', reader.result);
            var surrogate = reader.result.substring(0, reader.result.lastIndexOf(',') + 1);
            self.$avatar = reader.result.replace(surrogate, '');
        };
        reader.readAsDataURL(file);
    }

    , removeAvatar: function () {
        self.$avatar = '';
        self.$fupAvatar.value = '';
        self.$imgAvatar.setAttribute('src', 'interface/image/avatar.jpeg');
    }

    , resetPassword: function () {

        var width = self.$btnResetPassword.offsetWidth + 'px';
        self.$btnResetPassword.disabled = true;
        self.$btnResetPassword.innerHTML = '<i class="fa fa-spinner fa-lg w3-spin"></i>';
        self.$btnResetPassword.style.width = width;

        var xhttp;

        if (window.XMLHttpRequest) {
            xhttp = new XMLHttpRequest();
        } else {
            xhttp = new ActiveXObject('Microsoft.XMLHTTP');
        }

        xhttp.onreadystatechange = function () {
            if (this.readyState === 4 && (this.status === 200 || this.status === 204)) {
                Dialog.show
                    (
                        'Sucesso!'
                        , 'Senha redefinida e e-mail enviado.'
                        , Dialog.MessageType.SUCCESS
                        , Dialog.DisplayType.MODAL
                    );
                self.$btnResetPassword.disabled = false;
                $btnResetPassword.innerHTML = 'Redefinir senha';
            }

            if (this.readyState === 4 && this.status === 500) {
                Dialog.show
                    (
                        'Ops!'
                        , 'Ocorreu um problema.'
                        , Dialog.MessageType.ERROR
                        , Dialog.DisplayType.MODAL
                    );
                self.$btnResetPassword.disabled = false;
                $btnResetPassword.innerHTML = 'Redefinir senha';
            }
        };

        xhttp.open('put', Server.setService('api/users/reset-password/' + self.$UserID), true);
        xhttp.setRequestHeader('Content-type', 'application/json; charset=utf-8');
        xhttp.send(JSON.stringify(self.$user));

    }

};