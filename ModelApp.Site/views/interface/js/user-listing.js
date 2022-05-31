UserListing = {

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
            sessionStorage.setItem('UserID', 0);
            Utils.loadPage('user-maintenance.html');
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
                    UserListing.changeLoadingIcon();
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
                UserListing.setDataGrid(JSON.parse(this.responseText));
            }
        };

        try {
            xhttp.open('get', Server.setService('api/users'), true);
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

            var user = dataSource[i];

            var limitChar = 30;
            col1.innerHTML = user.Name;
            col2.innerHTML = user.UserRole.Name;
            col3.innerHTML = (user.Active ? 'Sim' : 'Não');
            col4.appendChild(this.rowCommand('update', user.UserID));
            col5.appendChild(this.rowCommand('delete', user.UserID));
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
                    UserListing.update(argument);
                });
                break;

            case 'delete':
                control.setAttribute('id', 'btnDelete_' + argument);
                faIcon.setAttribute('class', 'fas fa-trash-alt');
                faIcon.setAttribute('style', 'color:red');
                control.addEventListener('click', function () {
                    UserListing.delete(argument);
                });
                break;

            default:
                control = '';
                break;
        }

        return control;
    }

    , update: function (data) {
        sessionStorage.setItem('UserID', data);
        Utils.loadPage('user-maintenance.html');
    }

    , delete: function (data) {
        var op = confirm('Deseja excluir o usuário?');

        if (op) {
            var xhttp;

            if (window.XMLHttpRequest) {
                xhttp = new XMLHttpRequest();
            } else {
                xhttp = new ActiveXObject('Microsoft.XMLHTTP');
            }

            xhttp.onreadystatechange = function () {
                if (this.readyState === 4 && this.status === 200) {
                    UserRoleListing.cancel();
                }
            };

            xhttp.open('delete', Server.setService('api/users/' + data), true);
            xhttp.send();
        }
    }

    , cancel: function () {
        Utils.loadPage('user-listing.html');
    }

    , changeLoadingIcon: function () {
        document.querySelector('.fa, .fa-spinner, .fa-4x, .w3-spin').classList.remove('w3-spin');
        document.querySelector('.fa, .fa-spinner, .fa-4x, .w3-spin').classList.replace('fa-spinner', 'fa-exclamation-triangle');
    }
};