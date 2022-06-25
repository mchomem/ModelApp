Menu = {

    self: this

    , DeviceType: {
        MOBILE: 'Mobile'
        , BROWSER: 'Browser'
    }

    , init: function () {
        self.$menuContainer = document.getElementById('menuContainer');
        self.$menuContainerMob = document.getElementById('menuContainerMob');
        this.load();
    }

    , load: function () {
        var xhttp;
        if (window.XMLHttpRequest) {
            xhttp = new XMLHttpRequest();
        } else {
            xhttp = new ActiveXObject('Microsoft.XMLHTTP');
        }
        xhttp.onreadystatechange = function () {
            if (this.readyState === 4 && this.status === 200) {
                Menu.dismount(self.$menuContainer);
                Menu.dismount(self.$menuContainerMob);
                Menu.mount(JSON.parse(this.responseText), self.$menuContainer, self.$menuContainerMob);
            }
        };
        xhttp.open('get', Server.setService('api/menu'), true);
        xhttp.send();
    }

    , mount: function (data, container, containerMob) {
        if (data.length == 0) {
            Menu.noMenu();
            return;
        }
        for (var i = 0, l = data.length; i < l; i++) {
            var menu = data[i];
            if (!menu.Visible) {
                continue;
            }
            var menuItem = document.createElement('button');
            menuItem.setAttribute('type', 'button');
            menuItem.classList.add('w3-bar-item', 'w3-button', 'w3-indigo');
            menuItem.setAttribute('menu-item', menu.Label);
            menuItem.setAttribute('menu-parent', menu.ParentMenu != null ? menu.ParentMenu.Label : '');
            Menu.setIconAndLabel(menu, menuItem);
            if (menu.Page != null) {
                menuItem.setAttribute('page', menu.Page);
                menuItem.addEventListener('click', function () {
                    Menu.removeHighlight(Menu.DeviceType.BROWSER, self.$menuContainer);
                    Menu.addHighlight(Menu.DeviceType.BROWSER, this);
                    var page = this.getAttribute('page');
                    if (page != '#' && page != '' && page != null) {
                        if (page.includes('.html')) {
                            Utils.loadPage(this.getAttribute('page'));
                        }
                    }
                });
            }
            if (!menu.Active) {
                Menu.disableMenuItem(menuItem);
            }
            var mobMenuItem = menuItem.cloneNode(true);
            mobMenuItem.setAttribute('page', menu.Page);
            mobMenuItem.addEventListener('click', function () {
                self.$menuContainerMob.style.width = "0px";
                Menu.removeHighlight(Menu.DeviceType.BROWSER, self.$menuContainer);
                Menu.addHighlight(Menu.DeviceType.MOBILE, this);
                var mobPage = this.getAttribute('page');
                if (mobPage != '#' && mobPage != '' && mobPage != null) {
                    if (mobPage.includes('.html')) {
                        Utils.loadPage(this.getAttribute('page'));
                    }
                }
            });

            if (menu.ParentMenu == null) {
                if (menu.Page != '#') {
                    container.appendChild(menuItem);
                    containerMob.appendChild(mobMenuItem);
                }
            } else if (menu.ParentMenu != null) {
                if (menu.ParentMenu.Label == container.getAttribute('menu-parent')) {
                    container.appendChild(menuItem);
                    containerMob.appendChild(mobMenuItem);
                }
            }

            subMenuData = data.filter(function (item) {
                if (item.ParentMenu != null) {
                    return item.ParentMenu.MenuID == menu.MenuID;
                }
            });

            if (subMenuData.length > 0) {
                if (menu.Page == '#') {
                    var subMenus = document.createElement('div');
                    var containerSubMenu = document.createElement('div');
                    containerSubMenu.classList.add('w3-hide', 'w3-white', 'w3-card');
                    containerSubMenu.setAttribute('menu-parent', menu.Label);
                    var caret = document.createElement('i');
                    caret.classList.add('fa', 'fa-caret-left');
                    var button = document.createElement('button');
                    button.setAttribute('type', 'button');
                    Menu.setIconAndLabel(menu, button, caret);
                    button.classList.add('w3-bar-item', 'w3-button', 'w3-indigo');
                    button.addEventListener('click', function () {
                        Menu.openCloseSubMenu(this);
                    });
                    subMenus.appendChild(button);
                    subMenus.appendChild(containerSubMenu);

                    var subMenusMob = subMenus.cloneNode(true);
                    // Accesses the inner button (first element) of the mobile menu and attaches a click event!
                    subMenusMob.children[0].addEventListener('click', function () {
                        Menu.openCloseSubMenu(this);
                    });
                    container.appendChild(subMenus);
                    containerMob.appendChild(subMenusMob);
                    Menu.mount(subMenuData, containerSubMenu, subMenusMob.children[1]);
                }
            }
        }
    }

    , dismount: function (el) {
        while (el.firstChild) {
            Menu.dismount(el.firstChild);
            el.removeChild(el.firstChild);
        }
    }

    , setIconAndLabel: function (menu, menuItem, caret = undefined) {

        var row = document.createElement('div');
        row.classList.add('w3-row');
        var colIcon = document.createElement('div');
        colIcon.classList.add('w3-col', 's2', 'm2', 'l2');
        var colLabel = document.createElement('div');
        colLabel.classList.add('w3-col', 's9', 'm9', 'l9');
        var colCaret = document.createElement('div');
        colCaret.classList.add('w3-col', 's1', 'm1', 'l1');

        var icon;
        if (menu.ImageIcon != null && menu.ImageIcon.length > 0) {
            icon = document.createElement('img');
            icon.setAttribute('src', 'data:image/png;base64,' + menu.ImageIcon);
            icon.setAttribute('style', 'width:16px;height:16px;');
            colIcon.append(icon);
            colLabel.append(menu.Label);
        } else if (menu.CssFontAwesomeIcon.length > 0) {
            icon = document.createElement('i');
            var faClasses = menu.CssFontAwesomeIcon.split(' ');
            for (var x = 0; x < faClasses.length; x++) {
                icon.classList.add(faClasses[x]);
            }
            colIcon.append(icon);
            colLabel.append(menu.Label);
        } else {
            colLabel.append(menu.Label);
        }

        if (caret != undefined) {
            colCaret.append(caret);
        }

        row.appendChild(colIcon);
        row.appendChild(colLabel);
        row.appendChild(colCaret);
        menuItem.appendChild(row);
    }

    , noMenu: function () {
        var menuItem = document.createElement('a');
        menuItem.classList.add('w3-bar-item', 'w3-button', 'w3-khaki');
        menuItem.setAttribute('href', '#');        
        var icon = document.createElement('i');
        icon.classList.add('fas', 'fa-exclamation-circle');
        menuItem.appendChild(icon);
        menuItem.innerHTML = menuItem.innerHTML + ' Não há menu cadastrado';
        var mobMenuItem = menuItem.cloneNode(true);
        mobMenuItem.addEventListener('click', function () {
            self.$menuContainerMob.style.width = "0px";
        });
        self.$menuContainer.appendChild(menuItem);
        self.$menuContainerMob.appendChild(mobMenuItem);
    }

    , disableMenuItem: function (menuItem) {
        menuItem.setAttribute('disabled', 'disabled');
    }

    , openCloseSubMenu: function (menuContainer) {
        var cont = menuContainer.parentNode;
        var caret = menuContainer.children[0].children[2].children[0];
        if (!cont.children[1].classList.contains('w3-show')) {
            caret.classList.remove('fa-caret-left');
            caret.classList.add('fa-caret-down');
            cont.children[1].classList.add('w3-show');
        } else {
            caret.classList.add('fa-caret-left');
            caret.classList.remove('fa-caret-down');
            cont.children[1].classList.remove('w3-show');
        }
    }

    , orderByParent: function (data) {
        return data = data.sort(function (a, b) {
            if (a.ParentMenu != null && b.ParentMenu != null) {
                return a.ParentMenu.Order > b.ParentMenu.Order;
            }
        });
    }

    , addHighlight: function (device, el = undefined) {
        var menuContainer = el != undefined ? el.parentNode : self.$aHome.parentNode;
        for (var i = 0, l = menuContainer.children.length; i < l; i++) {
            menuContainer.children[i].classList.add('w3-indigo');
            menuContainer.children[i].classList.remove('w3-text-indigo');
        }
        if (el != undefined) {
            switch (device) {
                case Menu.DeviceType.BROWSER:
                    el.classList.remove('w3-indigo');
                    el.classList.add('w3-text-indigo');
                    break;
                default:
                    break;
            }
        }
    }

    , removeHighlight: function (device, el = null) {
        for (var i = 0, l = el.children.length; i < l; i++) {
            var tag = el.children[i];
            if (tag.tagName == 'BUTTON') {
                switch (device) {
                    case Menu.DeviceType.BROWSER:
                        tag.classList.add('w3-indigo');
                        tag.classList.remove('w3-text-indigo');
                        break;
                    default:
                        break;
                }
            }
            Menu.removeHighlight(device, tag);
        }        
    }
};