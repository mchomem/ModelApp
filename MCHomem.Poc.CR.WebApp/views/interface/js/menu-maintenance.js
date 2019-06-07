MenuMaintenance = {

    self: this

    , init: function () {
        self.$loading = document.querySelector('.loading');
        self.$frmMenuMaintenance = document.getElementById('frmMenuMaintenance');
        self.$txtLabel = document.getElementById('txtLabel');
        self.$chkIsParentMenu = document.getElementById('chkIsParentMenu');
        self.$txtPage = document.getElementById('txtPage');
        self.$selParentMenu = document.getElementById('selParentMenu');
        self.$rdbFile = document.getElementById('rdbFile');
        self.$rdbFile.checked = true;
        self.$rdbCssFa = document.getElementById('rdbCssFa');
        self.$rowFieldIcon = document.getElementById('rowFieldIcon');
        self.$rowFieldCssFontAwesome = document.getElementById('rowFieldCssFontAwesome');
        self.$fupIcon = document.getElementById('fupIcon');
        self.$imgImageIcon = document.getElementById('imgImageIcon');
        self.$txtCssFontAwesome = document.getElementById('txtCssFontAwesome');
        self.$iCssFontAwesomeSample = document.getElementById('iCssFontAwesomeSample');
        self.$chkActive = document.getElementById('chkActive');
        self.$chkActive.checked = true;
        self.$chkVisible = document.getElementById('chkVisible');
        self.$chkVisible.checked = true;
        self.$txtOrder = document.getElementById('txtOrder');
        self.$btnSave = document.getElementById('btnSave');
        self.$btnCancel = document.getElementById('btnCancel');

        this.attachEvent();
        this.toogleIconOptions();

        self.$MenuID = sessionStorage.getItem('MenuID');
        self.$menu = null;
        this.loadDataParentMenu();
        self.$parentMenu = null;
        self.$parentMenus = [];
        self.$ImageIcon = '';

        if (Number(self.$MenuID) != 0) {
            this.load(self.$MenuID);
        } else {
            MenuMaintenance.showForm();
        }
    }

    , attachEvent: function () {
        self.$chkIsParentMenu.addEventListener('click', function () {
            if (self.$txtPage.disabled) {
                self.$txtPage.disabled = false;
            } else {
                self.$txtPage.disabled = true;
                self.$txtPage.value = '';
            }
        });

        self.$selParentMenu.addEventListener('change', function () {
            var ParentMenuID = self.$selParentMenu.options[self.$selParentMenu.selectedIndex].value;
            for (var i = 0; i < self.$parentMenus.length; i++) {
                if (ParentMenuID == self.$parentMenus[i].MenuID) {
                    self.$parentMenu = self.$parentMenus[i];
                    break;
                }
                if (ParentMenuID == 0) {
                    self.$parentMenu = null;
                    break;
                }
            }
        });

        self.$rdbFile.addEventListener('click', function () {
            MenuMaintenance.toogleIconOptions('block', 'none');
            MenuMaintenance.removeImageIconCSSFA();
        });

        self.$rdbCssFa.addEventListener('click', function () {
            MenuMaintenance.toogleIconOptions('none', 'block');
            MenuMaintenance.removeImageIcon();
        });

        self.$fupIcon.addEventListener('change', function () {
            if (self.$fupIcon.files.length > 0) {
                if (!MenuMaintenance.checkImageExtension(self.$fupIcon)) {
                    self.$fupIcon.value = '';
                    Dialog.show('Aviso!', 'O arquivo não é uma imagem.', Dialog.MessageType.WARNING, Dialog.DisplayType.MODAL);
                    return;
                }
                MenuMaintenance.imageToBase64(self.$fupIcon);
            }
        });

        self.$txtCssFontAwesome.addEventListener('input', function () {
            MenuMaintenance.loadCSSFontAwesome(self.$txtCssFontAwesome.value);
        });

        self.$btnSave.addEventListener('click', function () {
            MenuMaintenance.save();
        });

        self.$btnCancel.addEventListener('click', function () {
            MenuMaintenance.cancel();
        });
    }

    , clear: function () {
        self.$txtLabel.value = '';
        self.$chkIsParentMenu.checked = false;
        self.$txtPage.value = '';
        self.$txtPage.disabled = false;
        self.$rdbFile.checked = true;
        self.$fupIcon.value = '';
        self.$txtCssFontAwesome.value = '';
        self.$iCssFontAwesomeSample.removeAttribute("class");
        self.$chkActive.checked = true;
        self.$chkVisible.checked = true;
        this.toogleIconOptions();
        self.$txtOrder.value = '';
    }

    , check: function () {
        var fields = [];
        var message = '';

        if (self.$txtLabel.value.length == 0)
            fields.push('Rótulo');

        if (self.$chkIsParentMenu.checked == false && self.$txtPage.value.length == 0)
            fields.push('Página');

        if (self.$txtOrder.value.length == 0)
            fields.push('Ordem');

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
                    if (Number(self.$MenuID) == 0) {
                        MenuMaintenance.clear();
                    }
                    Menu.init();
                    Dialog.show('Sucesso!', 'Registro ' + op, Dialog.MessageType.SUCCESS, Dialog.DisplayType.MODAL);
                }
            };

            if (Number(self.$MenuID) != 0) {

                if (self.$parentMenu != null) {
                    if (self.$parentMenu.Label === self.$menu.Label) {
                        Dialog.show('Aviso!', 'O menu pai escolhido é o próprio menu. Escolha um menu pai diferente!', Dialog.MessageType.WARNING, Dialog.DisplayType.MODAL);
                        self.$btnSave.disabled = false;
                        return;
                    }
                }

                var image;

                if ($menu.ImageIcon != null && $menu.ImageIcon.length > 0 && self.$ImageIcon.length == 0 ) {
                    image = $menu.ImageIcon;
                } else {
                    image = self.$ImageIcon;
                }

                if (self.$txtCssFontAwesome.value.length > 0) {
                    image = null;
                }

                var currentMenu = {
                    MenuID: Number(self.$MenuID)
                    , Label: self.$txtLabel.value
                    , Page: self.$chkIsParentMenu.checked ? '#' : self.$txtPage.value
                    , ParentMenuID: self.$parentMenu != null ? self.$parentMenu.MenuID : null
                    , ParentMenu: self.$parentMenu != null ? {
                        MenuID: self.$parentMenu.MenuID
                        , Label: self.$parentMenu.Label
                        , Page: self.$parentMenu.Page
                        , ParentMenuID: self.$parentMenu.MenuID
                        , ParentMenu: self.$parentMenu.ParentMenu
                        , ImageIcon: self.$parentMenu.ImageIcon
                        , CssFontAwesomeIcon: self.$parentMenu.CssFontAwesomeIcon
                        , Active: self.$parentMenu.Active
                        , Visible: self.$parentMenu.Visible
                        , Order: self.$parentMenu.Order
                        , CreatedBy: self.$parentMenu.CreatedBy
                        , CreatedIn: self.$parentMenu.CreatedIn
                        , UpdatedBy: self.$parentMenu.UpdatedBy
                        , UpdatedIn: self.$parentMenu.UpdatedIn
                    } : null
                    , ImageIcon: image
                    , CssFontAwesomeIcon: self.$txtCssFontAwesome.value
                    , Active: self.$chkActive.checked
                    , Visible: self.$chkVisible.checked
                    , Order: self.$txtOrder.value
                    , CreatedBy: self.$menu.CreatedBy
                    , CreatedIn: self.$menu.CreatedIn
                    , UpdatedBy: JSON.parse(sessionStorage.getItem('loginUser')).Name
                    , UpdatedIn: new Date()
                };

                op = 'atualizado.';
                data = JSON.stringify(currentMenu);
                xhttp.open('put', Server.setService('api/menus/' + self.$MenuID), true);
                xhttp.setRequestHeader('Content-type', 'application/json; charset=utf-8');
                xhttp.send(data);
            } else {
                var newMenu = {
                    MenuID: Number(self.$MenuID)
                    , Label: self.$txtLabel.value
                    , Page: self.$chkIsParentMenu.checked ? '#' : self.$txtPage.value
                    , ParentMenuID: self.$parentMenu != null ? self.$parentMenu.MenuID : null
                    , ParentMenu: self.$parentMenu != null ? {
                        MenuID: self.$parentMenu.MenuID
                        , Label: self.$parentMenu.Label
                        , Page: self.$parentMenu.Page
                        , ParentMenuID: self.$parentMenu.MenuID
                        , ParentMenu: self.$parentMenu.ParentMenu
                        , ImageIcon: self.$parentMenu.ImageIcon
                        , CssFontAwesomeIcon: self.$parentMenu.CssFontAwesomeIcon
                        , Active: self.$parentMenu.Active
                        , Visible: self.$parentMenu.Visible
                        , Order: self.$parentMenu.Order
                        , CreatedBy: self.$parentMenu.CreatedBy
                        , CreatedIn: self.$parentMenu.CreatedIn
                        , UpdatedBy: self.$parentMenu.UpdatedBy
                        , UpdatedIn: self.$parentMenu.UpdatedIn
                    } : null
                    , ImageIcon: self.$ImageIcon
                    , CssFontAwesomeIcon: self.$txtCssFontAwesome.value
                    , Active: self.$chkActive.checked
                    , Visible: self.$chkVisible.checked
                    , Order: self.$txtOrder.value
                    , CreatedBy: JSON.parse(sessionStorage.getItem('loginUser')).Name
                    , CreatedIn: new Date()
                };

                op = 'salvo.';
                data = JSON.stringify(newMenu);
                xhttp.open('post', Server.setService('api/menus'), true);
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

    , loadDataParentMenu: function () {
        var xhttp;

        if (window.XMLHttpRequest) {
            xhttp = new XMLHttpRequest();
        } else {
            xhttp = new ActiveXObject('Microsoft.XMLHTTP');
        }

        xhttp.onreadystatechange = function () {
            if (this.readyState === 4 && this.status === 200) {
                MenuMaintenance.populeParentMenu(JSON.parse(this.responseText));
            }
        };

        xhttp.open('get', Server.setService('api/menus'), true);
        xhttp.send();
    }

    , populeParentMenu: function (dataSource) {
        self.$selParentMenu.innerHTML = '';
        var opSel = document.createElement('option');
        opSel.value = '0';
        opSel.textContent = 'Selecione um menu pai';
        self.$selParentMenu.appendChild(opSel);

        for (var i = 0; i < dataSource.length; i++) {
            self.$parentMenus.push(dataSource[i]);
            var op = document.createElement('option');
            op.value = dataSource[i].MenuID;
            op.textContent = dataSource[i].Label;
            self.$selParentMenu.appendChild(op);
        }
    }

    , load: function (MenuID) {
        var xhttp;

        if (window.XMLHttpRequest) {
            xhttp = new XMLHttpRequest();
        } else {
            xhttp = new ActiveXObject('Microsoft.XMLHTTP');
        }

        xhttp.onreadystatechange = function () {
            if (this.readyState === 4 && this.status === 200) {
                self.$menu = JSON.parse(this.responseText);
                self.$txtLabel.value = self.$menu.Label;

                if (self.$menu.Page == '#') {
                    self.$chkIsParentMenu.checked = true;
                    self.$txtPage.disabled = true;
                } else {
                    self.$txtPage.value = self.$menu.Page;
                }

                if (self.$menu.ParentMenu != null) {
                    for (var i = 0; i < self.$selParentMenu.options.length; i++) {
                        if (Number(self.$selParentMenu.options[i].value) === self.$menu.ParentMenu.MenuID) {
                            self.$selParentMenu.options[i].defaultSelected = true;
                            break;
                        }
                    }

                    for (var x = 0; x < self.$parentMenus.length; x++) {
                        if (Number(self.$parentMenus[x].MenuID) === self.$menu.ParentMenu.MenuID) {
                            self.$parentMenu = self.$parentMenus[x];
                            break;
                        }
                    }
                }

                if (self.$menu.ImageIcon != null || self.$menu.ImageIcon != undefined) {
                    if (self.$menu.ImageIcon.length > 0) {
                        self.$imgImageIcon.setAttribute('src', 'data:image/png;base64,' + self.$menu.ImageIcon);
                        self.$rdbFile.checked = true;
                        MenuMaintenance.toogleIconOptions();
                    }
                }

                if (self.$menu.CssFontAwesomeIcon.length > 0) {
                    self.$txtCssFontAwesome.value = self.$menu.CssFontAwesomeIcon;
                    self.$rdbCssFa.checked = true;
                    MenuMaintenance.loadCSSFontAwesome(self.$menu.CssFontAwesomeIcon);
                    MenuMaintenance.toogleIconOptions('none', 'block');
                }

                self.$chkActive.checked = self.$menu.Active;
                self.$chkVisible.checked = self.$menu.Visible;
                self.$txtOrder.value = self.$menu.Order;
                MenuMaintenance.showForm();   
            }
        };

        xhttp.open('get', Server.setService('api/menus/' + MenuID), true);
        xhttp.send();
    }

    , cancel: function () {
        Utils.loadPage('menu-listing.html');
    }

    , showForm: function () {
        self.$loading.classList.add('w3-hide');
        self.$frmMenuMaintenance.classList.remove('w3-hide');
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
            self.$imgImageIcon.setAttribute('src', reader.result);
            var surrogate = reader.result.substring(0, reader.result.lastIndexOf(',') + 1);
            self.$ImageIcon = reader.result.replace(surrogate, '');
        };
        reader.readAsDataURL(file);
    }

    , removeImageIcon: function () {
        self.$fupIcon.value = '';
        self.$ImageIcon = '';
        self.$imgImageIcon.setAttribute('src', '');
    }

    , removeImageIconCSSFA: function () {
        self.$txtCssFontAwesome.value = '';
        self.$iCssFontAwesomeSample.removeAttribute('class');
        self.$iCssFontAwesomeSample.classList.add('w3-text-indigo');
    }

    , toogleIconOptions: function (displayRowIconImage = 'block', displayRowIconCssFA = 'none') {
        self.$rowFieldIcon.style.display = displayRowIconImage;
        self.$rowFieldCssFontAwesome.style.display = displayRowIconCssFA;
    }

    , loadCSSFontAwesome: function (source) {
        try {
            self.$iCssFontAwesomeSample.removeAttribute('class');
            self.$iCssFontAwesomeSample.classList.add('w3-text-indigo');
            if (source.length != 0) {
                var faClasses = source.split(' ');
                for (var i = 0; i < faClasses.length; i++) {
                    if (faClasses[i].length != 0) {
                        self.$iCssFontAwesomeSample.classList.add(faClasses[i]);
                    }
                }
            }
        } catch (e) {
            Dialog.show
            (
                'Ops!'
                , 'Não foi possível carregar a classe Font Awesome.'
                , Dialog.MessageType.ERROR
                , Dialog.DisplayType.MODAL
            );
        }
        
    }

};