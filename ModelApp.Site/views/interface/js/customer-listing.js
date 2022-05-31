CustomerListing = {

    self: this

    , init: function () {
        self.loading = document.querySelector('.loading');
        self.$btnNew = document.getElementById('btnNew');
        self.$grid = document.getElementById('grid');
        this.attachEvent();
        this.loadData();
    }

    , attachEvent: function () {
        self.$btnNew.addEventListener('click', function () {
            sessionStorage.setItem('CustomerID', 0);
            Utils.loadPage('customer-maintenance.html');
        });
    }

    , loadData: function () {
        var xhttp;

        if (window.XMLHttpRequest) {
            xhttp = new XMLHttpRequest();
        } else {
            xhttp = new ActiveXObject('Microsoft.XMLHTTP');
        }

        xhttp.onreadystatechange = function () {
            if (this.status === 500) {
                if (this.responseText != '') {
                    var response = JSON.parse(this.responseText);
                    CustomerListing.changeLoadingIcon();
                    Dialog.show
                    (
                        'Ops!'
                        , 'Ocorreu um problema. Mensagem do sistema: ' + this.statusText + ' (' + this.status + ') - ' + response.InnerException.ExceptionMessage
                        , Dialog.MessageType.ERROR
                        , Dialog.DisplayType.MODAL
                    );
                }
            }

            if (this.readyState === 4 && this.status === 200) {
                CustomerListing.setDataGrid(JSON.parse(this.responseText));
            }
        };

        try {
            xhttp.open('get', Server.setService('api/customers'), true);
            xhttp.send();
        } catch (e) {
            Dialog.show
                (
                    'Ops!'
                    , 'Ocorreu um problema.  Mensagem do sistema: ' + e.message + '. Stacktrace: ' + e.stacktrace
                    , Dialog.MessageType.ERROR
                    , Dialog.DisplayType.MODAL
                );
        }
    }

    , setDataGrid: function (dataSource) {
        var grid = document.getElementById('grid');
        self.loading.classList.add('w3-hide');

        if (dataSource.length == 0) {
            var nodata = document.querySelector('#nodata');
            nodata.classList.remove('w3-hide');
            return;
        }

        for (var i = 0; i < dataSource.length; i++) {
            var row = grid.insertRow(i + 1);
            var col1 = row.insertCell(0);
            var col2 = row.insertCell(1);
            var col3 = row.insertCell(2);
            var col4 = row.insertCell(3);
            var col5 = row.insertCell(4);
            var col6 = row.insertCell(5);
            var col7 = row.insertCell(6);

            var customer = dataSource[i];

            var limitChar = 30;
            col1.innerHTML = customer.Name;
            col2.innerHTML = moment(customer.DateBirth).format('DD[/]MM[/]YYYY');
            col3.innerHTML = customer.PhoneNumber;
            col4.innerHTML = (customer.Address.length >= limitChar ? customer.Address.substring(0, limitChar) + ' ...' : customer.Address);
            col5.innerHTML = (customer.Active ? 'Sim' : 'Não');
            col6.appendChild(this.rowCommand('update', customer.CustomerID));
            col7.appendChild(this.rowCommand('delete', customer.CustomerID));
        }
    }

    , rowCommand: function (command, argument) {
        var control;
        control = document.createElement('button');
        control.setAttribute('data-entity-id', argument);
        control.setAttribute('type', 'button');
        control.setAttribute('class', 'w3-btn');

        var faIcon = document.createElement('i');
        control.appendChild(faIcon);

        switch (command) {
            case 'update':
                control.setAttribute('id', 'btnUpdate_' + argument);
                faIcon.setAttribute('class', 'fas fa-pen');
                control.addEventListener('click', function () {
                    CustomerListing.update(argument);
                });
                break;

            case 'delete':
                control.setAttribute('id', 'btnDelete_' + argument);
                faIcon.setAttribute('class', 'fas fa-trash-alt');
                faIcon.setAttribute('style', 'color:red');
                control.addEventListener('click', function () {
                    CustomerListing.delete(argument);
                });
                break;

            default:
                control = '';
                break;
        }

        return control;
    }

    , update: function (data) {
        sessionStorage.setItem('CustomerID', data);
        Utils.loadPage('customer-maintenance.html');
    }

    , delete: function (data) {
        var op = confirm('Deseja excluir o cliente?');

        if (op) {
            var xhttp;

            if (window.XMLHttpRequest) {
                xhttp = new XMLHttpRequest();
            } else {
                xhttp = new ActiveXObject('Microsoft.XMLHTTP');
            }

            xhttp.onreadystatechange = function () {
                if (this.readyState === 4 && this.status === 200) {
                    CustomerListing.cancel();
                }
            };

            xhttp.open('delete', Server.setService('api/customers/' + data), true);
            xhttp.send();
        }
    }

    , cancel: function () {
        Utils.loadPage('customer-listing.html');
    }

    , changeLoadingIcon: function () {
        document.querySelector('.fa, .fa-spinner, .fa-4x, .w3-spin').classList.remove('w3-spin');
        document.querySelector('.fa, .fa-spinner, .fa-4x, .w3-spin').classList.replace('fa-spinner', 'fa-exclamation-triangle');
    }

};