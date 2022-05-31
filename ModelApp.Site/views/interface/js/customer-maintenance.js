CustomerMaintenance = {

    self: this

    , init: function () {
        self.$loading = document.querySelector('.loading');
        self.$frmCustomerMaintenance = document.getElementById('frmCustomerMaintenance');
        self.$txtName = document.getElementById('txtName');
        self.$txtDateBirth = document.getElementById('txtDateBirth');
        self.$txtPhoneNumber = document.getElementById('txtPhoneNumber');
        self.$txtAddress = document.getElementById('txtAddress');
        self.$chkActive = document.getElementById('chkActive');
        self.$chkActive.checked = true;
        self.$btnSave = document.getElementById('btnSave');
        self.$btnCancel = document.getElementById('btnCancel');
        this.attachEvent();

        self.$CustomerID = sessionStorage.getItem('CustomerID');
        self.$customer = null;

        if (Number(self.$CustomerID) != 0) {
            this.load(self.$CustomerID);
        } else {
            CustomerMaintenance.showForm();
        }
    }

    , attachEvent: function () {
        self.$btnSave.addEventListener('click', function () {
            CustomerMaintenance.save();
        });

        self.$btnCancel.addEventListener('click', function () {
            CustomerMaintenance.cancel();
        });

        self.$txtDateBirth.addEventListener('keyup', function (e) {
            Utils.mask(self.$txtDateBirth, '00/00/0000', e);
        });

        self.$txtPhoneNumber.addEventListener('keyup', function (e) {
            Utils.mask(self.$txtPhoneNumber, '(00)00000-0000', e);
        });
    }

    , clear: function () {
        self.$txtName.value = '';
        self.$txtDateBirth.value = '';
        self.$txtPhoneNumber.value = '';
        self.$txtAddress.value = '';
        self.$chkActive.checked = false;
    }

    , check: function () {

        var fields = [];
        var message = '';

        if (self.$txtName.value.length == 0)
            fields.push('Nome');

        if (self.$txtDateBirth.value.length == 0)
            fields.push('Data de aniversário');

        if (self.$txtPhoneNumber.value.length == 0)
            fields.push('Número de telefone');

        if (self.$txtAddress.value.length == 0)
            fields.push('Endereço');

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

            if (!moment(self.$txtDateBirth.value, 'DD/MM/YYYY').isValid()) {
                Dialog.show('Aviso!', 'Data inválida.', Dialog.MessageType.WARNING, Dialog.DisplayType.MODAL);
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
                    if (Number(self.$CustomerID) == 0) {
                        CustomerMaintenance.clear();
                    }
                    Dialog.show('Sucesso!', 'Registro ' + op, Dialog.MessageType.SUCCESS, Dialog.DisplayType.MODAL);
                }
            };
            
            if (Number(self.$CustomerID) != 0) {
                var currentCustomer = {
                    CustomerID: Number(self.$CustomerID)
                    , Name: self.$txtName.value
                    , DateBirth: moment(self.$txtDateBirth.value, 'DD/MM/YYYY').format('YYYY[-]MM[-]DD')
                    , PhoneNumber: self.$txtPhoneNumber.value
                    , Address: self.$txtAddress.value
                    , Active: self.$chkActive.checked
                    , CreatedBy: self.$customer.CreatedBy
                    , CreatedIn: self.$customer.CreatedIn
                    , UpdatedBy: JSON.parse(sessionStorage.getItem('loginUser')).Name
                    , UpdatedIn: new Date()
                };

                op = 'atualizado.';
                data = JSON.stringify(currentCustomer);
                xhttp.open('put', Server.setService('api/customers/' + self.$CustomerID), true);
                xhttp.setRequestHeader('Content-type', 'application/json; charset=utf-8');
                xhttp.send(data);
            } else {
                var newCustomer = {
                    CustomerID: Number(self.$CustomerID)
                    , Name: self.$txtName.value
                    , DateBirth: moment(self.$txtDateBirth.value, 'DD/MM/YYYY').format('YYYY[-]MM[-]DD')
                    , PhoneNumber: self.$txtPhoneNumber.value
                    , Address: self.$txtAddress.value
                    , Active: self.$chkActive.checked
                    , CreatedBy: JSON.parse(sessionStorage.getItem('loginUser')).Name
                    , CreatedIn: new Date()
                };

                op = 'salvo.';
                data = JSON.stringify(newCustomer);
                xhttp.open('post', Server.setService('api/customers'), true);
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

    , load: function (CustomerID) {
        var xhttp;

        if (window.XMLHttpRequest) {
            xhttp = new XMLHttpRequest();
        } else {
            xhttp = new ActiveXObject('Microsoft.XMLHTTP');
        }

        xhttp.onreadystatechange = function () {
            if (this.readyState === 4 && (this.status === 200)) {
                self.$customer = JSON.parse(this.responseText);
                self.$txtName.value = self.$customer.Name;
                self.$txtDateBirth.value = moment(self.$customer.DateBirth).format('DD[/]MM[/]YYYY');
                self.$txtPhoneNumber.value = self.$customer.PhoneNumber;
                self.$txtAddress.value = self.$customer.Address;
                self.$chkActive.checked = self.$customer.Active;
                CustomerMaintenance.showForm();
            }
        };

        xhttp.open('get', Server.setService('api/customers/' + CustomerID), true);
        xhttp.send();
    }

    , cancel: function () {
        Utils.loadPage('customer-listing.html');
    }

    , showForm: function () {
        self.$loading.classList.add('w3-hide');
        self.$frmCustomerMaintenance.classList.remove('w3-hide');
    }

};