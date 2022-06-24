Login = {

    self: this

    , init: function () {

        self.$user;
        self.$txtUser = document.getElementById('txtUser');
        self.$txtUser.focus();
        self.$txtPassword = document.getElementById('txtPassword');
        self.$chkRemember = document.getElementById('chkRemember');
        self.$aIForgotMyPassword = document.getElementById('aIForgotMyPassword');
        self.$btnLogin = document.getElementById('btnLogin');
        self.$loading = document.querySelector('.loading');

        // Modal controls.
        self.$modalForm = document.getElementById('modalForm');
        self.$txtSecretPhrase = document.getElementById('txtSecretPhrase');
        self.$btnCheckSecretPhrase = document.getElementById('btnCheckSecretPhrase');
        self.$strUserName = document.getElementById('strUserName');
        self.$divCheckSecretPhrase = document.getElementById('divCheckSecretPhrase');
        self.$divChangePassword = document.getElementById('divChangePassword');
        self.$txtNewPassword = document.getElementById('txtNewPassword');
        self.$txtConfirmNewPassword = document.getElementById('txtConfirmNewPassword');
        self.$btnUpdatePassword = document.getElementById('btnUpdatePassword');
        self.$btnCloseModal = document.getElementById('btnCloseModal');

        this.attachEvent();
        this.grantAccessFromCookie();
    }

    , attachEvent: function () {
        self.$btnLogin.addEventListener('click', function () {
            Login.grantAccessFromForm();
        });

        document.onkeydown = function (e) {
            e = e || window.event;
            if (e.keyCode == 13) {
                Login.grantAccessFromForm();
            }
        };

        self.$aIForgotMyPassword.addEventListener('click', function () {

            if (self.$txtUser.value.length == 0 || self.$txtPassword.value.length == 0) {
                Dialog.show('Aviso!', 'Tente fazer login com o seu usuário!', Dialog.MessageType.WARNING, Dialog.DisplayType.BOX);
                return;
            }

            self.$modalForm.style.display = 'block';
        });

        self.$btnCheckSecretPhrase.addEventListener('click', function () {
            Login.checkSecretPhrase();
        });

        self.$btnUpdatePassword.addEventListener('click', function () {
            Login.updatePassword();
        });

        self.$btnCloseModal.addEventListener('click', function () {
            self.$modalForm.style.display = 'none';
            Login.resetPagesModal();
        });
    }

    , clear: function () {
        self.$txtUser.value = '';
        self.$txtPassword.value = '';
        self.$chkRemember.checked = false;
    }

    , resetPagesModal: function () {
        self.$divCheckSecretPhrase.classList.remove('w3-hide');
        self.$divChangePassword.classList.add('w3-hide');
        self.$txtSecretPhrase.value = '';
        self.$txtNewPassword.value = '';
        self.$txtConfirmNewPassword.value = '';
        self.$btnCloseModal.innerText = 'Cancelar';
    }

    , check: function () {
        var fields = [];
        var message = '';

        if (self.$txtUser.value.length == 0)
            fields.push('Nome');

        if (self.$txtPassword.value.length == 0)
            fields.push('Senha');

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

    , grantAccessFromForm: function () {
        var check = Login.check();

        if (check.length > 0) {
            Dialog.show('Aviso!', check, Dialog.MessageType.WARNING, Dialog.DisplayType.BOX);
            return;
        }

        self.$btnLogin.disabled = true;
        self.$loading.classList.remove('w3-hide');

        var xhttp;

        if (window.XMLHttpRequest) {
            xhttp = new XMLHttpRequest();
        } else {
            xhttp = new ActiveXObject('Microsoft.XMLHTTP');
        }

        xhttp.onreadystatechange = function () {
            if (this.readyState === 4 && this.status === 200) {
                if (self.$chkRemember.checked) {
                    var daysToExpirate = 2;
                    Utils.setCookie('cr-user', self.$txtUser.value, daysToExpirate);
                    Utils.setCookie('cr-password', self.$txtPassword.value, daysToExpirate);
                } else {
                    Utils.setCookie('cr-user', '', 0);
                    Utils.setCookie('cr-password', '', 0);
                }
                sessionStorage.setItem('loginUser', this.responseText);
                window.location.assign('master.html');
            }

            if (this.status === 500) {
                self.$btnLogin.disabled = false;
                self.$loading.classList.add('w3-hide');
                Dialog.show('Ops!', 'Sistema indisponível! Contate o administrador de sistemas.', Dialog.MessageType.ERROR, Dialog.DisplayType.BOX);
            }

            if (this.readyState === 4 && this.status === 0) {
                self.$btnLogin.disabled = false;
                self.$loading.classList.add('w3-hide');
                Dialog.show('Ops!', 'Problema na comunição com o sistema!', Dialog.MessageType.ERROR, Dialog.DisplayType.BOX);
            }

            if (this.readyState === 4 && this.status === 404) {
                self.$btnLogin.disabled = false;
                self.$loading.classList.add('w3-hide');
                Dialog.show('Ops!', 'Usuário ou senha inválidos!', Dialog.MessageType.WARNING, Dialog.DisplayType.BOX);
            }
        };

        xhttp.open('get', Server.setService('api/user/authentication?login=' + self.$txtUser.value + '&password=' + self.$txtPassword.value), true);
        xhttp.send();
    }

    , grantAccessFromCookie: function () {
        var user = Utils.getCookie('cr-user');
        var password = Utils.getCookie('cr-password');

        if (user.length > 0) {
            self.$loading.classList.remove('w3-hide');
            var xhttp;

            if (window.XMLHttpRequest) {
                xhttp = new XMLHttpRequest();
            } else {
                xhttp = new ActiveXObject('Microsoft.XMLHTTP');
            }

            xhttp.onreadystatechange = function () {
                if (this.readyState === 4 && this.status === 200) {
                    sessionStorage.setItem('loginUser', this.responseText);
                    window.location.assign('master.html');
                }

                if (this.status === 500) {
                    self.$btnLogin.disabled = false;
                    self.$loading.classList.add('w3-hide');
                    Dialog.show('Ops!', 'Sistema indisponível! Contate o administrador de sistemas.', Dialog.MessageType.ERROR, Dialog.DisplayType.BOX);
                }

                if (this.readyState === 4 && this.status === 0) {
                    self.$btnLogin.disabled = false;
                    self.$loading.classList.add('w3-hide');
                    Dialog.show('Ops!', 'Problema na comunição com o sistema!', Dialog.MessageType.ERROR, Dialog.DisplayType.BOX);
                }

                if (this.status === 404) {
                    self.$btnLogin.disabled = false;
                    self.$loading.classList.add('w3-hide');
                    Dialog.show('Ops!', 'Usuário ou senha inválidos!', Dialog.MessageType.WARNING, Dialog.DisplayType.BOX);
                }
            };

            xhttp.open('get', Server.setService('api/user/authentication?login=' + user + '&password=' + password), true);
            xhttp.send();
        }
    }

    , checkSecretPhrase: function () {

        // Showing a feedback to user...
        var width = self.$btnCheckSecretPhrase.offsetWidth + 'px';
        self.$btnCheckSecretPhrase.disabled = true;
        self.$btnCheckSecretPhrase.innerHTML = '<i class="fa fa-spinner fa-lg w3-spin"></i>';
        self.$btnCheckSecretPhrase.style.width = width;

        if (self.$txtSecretPhrase.value.length === 0) {
            Dialog.show('Aviso', 'Informe a frase secreta antes de verifcar.', Dialog.MessageType.WARNING, Dialog.DisplayType.BOX, 'dialogContainerModal');
            return;
        }

        var user = self.$txtUser.value;
        var secretPhrase = self.$txtSecretPhrase.value;
        var xhttp;

        if (window.XMLHttpRequest) {
            xhttp = new XMLHttpRequest();
        } else {
            xhttp = new ActiveXObject('Microsoft.XMLHTTP');
        }

        xhttp.onreadystatechange = function () {
            if (this.readyState === 4 && this.status === 200) {
                self.$user = JSON.parse(this.responseText);
                self.$strUserName.innerText = user;
                self.$divCheckSecretPhrase.classList.add('w3-hide');
                self.$divChangePassword.classList.remove('w3-hide');
            }

            if (this.readyState === 4 && this.status === 404) {
                self.$btnCheckSecretPhrase.disabled = false;
                self.$btnCheckSecretPhrase.innerHTML = 'Verificar';
                Dialog.show('Aviso', 'Frase secreta incorreta! Não será possível redefinir a sua nova senha.', Dialog.MessageType.WARNING, Dialog.DisplayType.BOX, 'dialogContainerModal');
            }
        };

        xhttp.open('get', Server.setService('api/users/GetUserSecretPhrase?name=' + user + '&secretPhrase=' + secretPhrase), true);
        xhttp.send();
    }

    , updatePassword: function () {

        if (self.$txtNewPassword.value.length === 0 || self.$txtConfirmNewPassword.value.length === 0) {
            Dialog.show('Aviso', 'Informe os campos de senha e confirmação de senha.', Dialog.MessageType.WARNING, Dialog.DisplayType.BOX, 'dialogContainerModal');
            return;
        }

        if (self.$txtNewPassword.value !== self.$txtConfirmNewPassword.value) {
            Dialog.show('Aviso', 'As senhas não conferem.', Dialog.MessageType.WARNING, Dialog.DisplayType.BOX, 'dialogContainerModal');
            return;
        }

        var xhttp;

        if (window.XMLHttpRequest) {
            xhttp = new XMLHttpRequest();
        } else {
            xhttp = new ActiveXObject('Microsoft.XMLHTTP');
        }

        xhttp.onreadystatechange = function () {
            if (this.readyState === 4 && (this.status === 200 || this.status === 204)) {
                Dialog.show('Sucesso!', 'Senha redefinida!', Dialog.MessageType.SUCCESS, Dialog.DisplayType.BOX, 'dialogContainerModal');
                self.$btnCloseModal.innerText = 'Fechar';
            }

            if (this.readyState === 4 && this.status === 500) {
                Dialog.show('Ops!', 'Não foi possível redefinir sua senha. Contate o administrador de sistema.', Dialog.MessageType.ERROR, Dialog.DisplayType.BOX, 'dialogContainerModal');
            }
        };

        self.$user.Password = self.$txtNewPassword.value;

        var data = JSON.stringify(self.$user);
        xhttp.open('put', Server.setService('api/users/' + self.$user.UserID), true);
        xhttp.setRequestHeader('Content-type', 'application/json; charset=utf-8');
        xhttp.send(data);
    }

};
