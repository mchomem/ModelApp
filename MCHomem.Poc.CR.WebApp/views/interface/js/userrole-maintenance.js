UserRoleMaintenance = {

    self: this

    , init: function () {
        self.$loading = document.querySelector('.loading');
        self.$frmUserRoleMaintenance = document.getElementById('frmUserRoleMaintenance');
        self.$txtName = document.getElementById('txtName');
        self.$txtDescription = document.getElementById('txtDescription');
        self.$btnSave = document.getElementById('btnSave');
        self.$btnCancel = document.getElementById('btnCancel');
        this.attachEvent();

        self.$UserRoleID = sessionStorage.getItem('UserRoleID');
        self.$userRole = null;

        if (Number(self.$UserRoleID) != 0) {
            this.load(self.$UserRoleID);
        } else {
            UserRoleMaintenance.showForm();
        }
    }

    , attachEvent: function () {
        self.$btnSave.addEventListener('click', function () {
            UserRoleMaintenance.save();
        });

        self.$btnCancel.addEventListener('click', function () {
            UserRoleMaintenance.cancel();
        });
    }

    , clear: function () {
        self.$txtName.value = '';
        self.$txtDescription.value = '';
    }

    , check: function () {

        var fields = [];
        var message = '';

        if (self.$txtName.value.length == 0)
            fields.push('Nome');

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
                    if (Number(self.$UserRoleID) == 0) {
                        UserRoleMaintenance.clear();
                    }
                    Dialog.show('Sucesso!', 'Registro ' + op, Dialog.MessageType.SUCCESS, Dialog.DisplayType.MODAL);
                }
            };
            
            if (Number(self.$UserRoleID) != 0) {
                var currentUserRole = {
                    UserRoleID: Number(self.$UserRoleID)
                    , Name: self.$txtName.value
                    , Description: self.$txtDescription.value
                    , CreatedBy: self.$userRole.CreatedBy
                    , CreatedIn: self.$userRole.CreatedIn
                    , UpdatedBy: JSON.parse(sessionStorage.getItem('loginUser')).Name
                    , UpdatedIn: new Date()
                };

                op = 'atualizado.';
                data = JSON.stringify(currentUserRole);
                xhttp.open('put', Server.setService('api/userroles/' + self.$UserRoleID), true);
                xhttp.setRequestHeader('Content-type', 'application/json; charset=utf-8');
                xhttp.send(data);
            } else {
                var newUserRole = {
                    UserRoleID: Number(self.$UserRoleID)
                    , Name: self.$txtName.value
                    , Description: self.$txtDescription.value
                    , CreatedBy: JSON.parse(sessionStorage.getItem('loginUser')).Name
                    , CreatedIn: new Date()
                };

                op = 'salvo.';
                data = JSON.stringify(newUserRole);
                xhttp.open('post', Server.setService('api/userroles'), true);
                xhttp.setRequestHeader('Content-type', 'application/json; charset=utf-8');
                xhttp.send(data);
            }

        } catch (e) {
            Dialog.show
                (
                'Ops!'
                , 'Ocorreu um problema. Mensagem do sistema: ' + e.message + '. Stacktrace: ' + e.stacktrace
                , Dialog.MessageType.ERROR
                , Dialog.DisplayType.MODAL
                );
        }
    }

    , load: function (UserRoleID) {
        var xhttp;

        if (window.XMLHttpRequest) {
            xhttp = new XMLHttpRequest();
        } else {
            xhttp = new ActiveXObject('Microsoft.XMLHTTP');
        }

        xhttp.onreadystatechange = function () {
            if (this.readyState === 4 && (this.status === 200)) {
                self.$userRole = JSON.parse(this.responseText);
                self.$txtName.value = self.$userRole.Name;
                self.$txtDescription.value = self.$userRole.Description;
                UserRoleMaintenance.showForm();
            }
        };

        xhttp.open('get', Server.setService('api/userroles/' + UserRoleID), true);
        xhttp.send();
    }

    , cancel: function () {
        Utils.loadPage('userrole-listing.html');
    }

    , showForm: function () {
        self.$loading.classList.add('w3-hide');
        self.$frmUserRoleMaintenance.classList.remove('w3-hide');
    }

};