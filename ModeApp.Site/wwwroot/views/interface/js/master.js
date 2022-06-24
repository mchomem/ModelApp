Master = {

    self: this

    , init: function () {
        self.$btnMobMenuBar = document.getElementById('btnMobMenuBar');
        self.$menuContainerMob = document.getElementById('menuContainerMob');
        self.$mobaHome = document.getElementById('mobaHome');
        self.$mobaCustomerListining = document.getElementById('mobaCustomerListining');
        self.$mobaUserListining = document.getElementById('mobaUserListining');
        self.$mobaUserRoleListining = document.getElementById('mobaUserRoleListining');
        self.$mobaMenuListining = document.getElementById('mobaMenuListining');

        self.$asMenu = document.getElementById('asMenu');
        self.$btnLogout = document.getElementById('btnLogout');
        self.$spanUser = document.getElementById('spanUser');

        self.$btnUserData = document.getElementById('btnUserData');

        self.$btnNewLogin = document.getElementById('btnNewLogin');
        self.$toHome = document.getElementById('toHome');
        self.$aHome = document.getElementById('aHome');
        self.$aCustomerListining = document.getElementById('aCustomerListining');
        self.$aUserListining = document.getElementById('aUserListining');
        self.$aUserRoleListining = document.getElementById('aUserRoleListining');
        self.$aMenuListining = document.getElementById('aMenuListining');
        this.attachEvent();
        this.getUserSessionLogin();
        this.enableControlChangeUser();
        this.currentPage();
    }

    , attachEvent: function () {
        self.$btnMobMenuBar.addEventListener('click', function () {
            self.$menuContainerMob.style.width = "80%";
        });

        self.$btnNewLogin.addEventListener('click', function () {
            Master.changeUser();
        });

        self.$btnUserData.addEventListener('click', function () {
            // TODO show a modal form with user data
        });

        self.$btnLogout.addEventListener('click', function () {
            Master.logout();
        });

        self.$toHome.addEventListener('click', function () {
            Utils.loadPage('home.html');
        });
    }

    , getUserSessionLogin: function () {
        var user = JSON.parse(sessionStorage.getItem('loginUser'));
        if (user == null) {
            window.location.assign('login.html');
        } else {
            self.$spanUser.innerText = user.Name.length > 10 ? user.Name.substring(0, 10) + '...' : user.Name;
        }
    }

    , changeUser: function () {
        var op = confirm('Deseja logar com outro usuário?');
        if (op) {
            Utils.setCookie('cr-user', '', 0);
            Utils.setCookie('cr-password', '', 0);
            Master.logout();
        }
    }

    , enableControlChangeUser: function () {
        var user = Utils.getCookie('cr-user');
        if (user.length > 0) {
            self.$btnNewLogin.classList.remove('w3-hide');
            self.$btnLogout.disabled = true;
        } else {
            self.$btnNewLogin.classList.add('w3-hide');
            self.$btnLogout.disabled = false;
        }
    }

    , logout: function () {
        sessionStorage.clear();
        window.location.assign('login.html');
    }

    , currentPage: function () {
        var page = sessionStorage.getItem('currentPage');
        if (page != null) {
            Utils.loadPage(page);
        }
    }
};