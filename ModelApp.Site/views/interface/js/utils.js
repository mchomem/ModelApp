Utils = {

    loadPage: function (page) {

        var xhttp;

        if (window.XMLHttpRequest) {
            xhttp = new XMLHttpRequest();
        } else {
            xhttp = new ActiveXObject('Microsoft.XMLHTTP');
        }

        xhttp.onreadystatechange = function () {
            if (this.readyState === 4 && this.status === 200) {
                document.getElementById('content').innerHTML = xhttp.responseText;
                Utils.execJS(document.getElementById('content'));
                sessionStorage.setItem('currentPage', page);
            } else if (this.readyState === 4 && this.status === 404) {
                console.log('Status: ' + this.statusText + ' - ' + this.responseURL);
                Utils.loadPage('404.html');
            }
        };

        xhttp.open('get', page, true);
        xhttp.send();
        console.log('Page ' + page + ' loaded');
    }

    , execJS: function (content) {
        var s = content.querySelector('script');
        if (s != null || s != undefined) {
            eval(s.innerHTML);
        } else {
            console.log('Tag <script> not found in content!');
        }
    }

    // ref https://www.w3schools.com/js/js_cookies.asp
    , setCookie: function (cname, cvalue, exdays) {
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
    }

    , getCookie: function (cname) {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }


    // http://giorgetti.com.br/blog/2011/12/07/simple-input-mask-using-javascript/
    , mask: function (inputText, mask, evt) {
        try {
            var value = inputText.value;
            // If user pressed DEL or BACK SPACE, clean the value
            try {
                var e = (evt.which) ? evt.which : event.keyCode;
                if (e == 46 || e == 8) {
                    inputText.value = "";
                    return;
                }
            } catch (e1) { }

            var literalPattern = /[0\*]/;
            var numberPattern = /[0-9]/;
            var newValue = "";

            for (var vId = 0, mId = 0; mId < mask.length;) {
                if (mId >= value.length)
                    break;

                // Number expected but got a different value, store only the valid portion
                if (mask[mId] == '0' && value[vId].match(numberPattern) == null) {
                    break;
                }

                // Found a literal
                while (mask[mId].match(literalPattern) == null) {
                    if (value[vId] == mask[mId])
                        break;

                    newValue += mask[mId++];
                }

                newValue += value[vId++];
                mId++;
            }

            inputText.value = newValue;
        } catch (e) { }
    }

};