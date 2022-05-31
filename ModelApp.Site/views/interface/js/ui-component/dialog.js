/*
 * Author: Misael C. Homem
 * Description: a custom alert (dialogs) with vanilla javascript, using the style library named w3.css from w3schools.com/w3css.
 * Version: 1.2.0.0.
 * Updates:
 * 30/05/2019 - added capability to choose any html element to serve how container.
 * 25/08/2018 - add event press key 'esc' to close both dialogs type.
 * 17/08/2018 - set focus on OK button.
 * 16/08/2018 - add option to display in box or modal.
 */

Dialog = {

    self: this

    , containerId: undefined

    , MessageType: {
        SUCCESS: 'Success'
        , WARNING: 'Warning'
        , ERROR: 'Error'
        , INFORMATION: 'Information'
    }

    , DisplayType: {    
        BOX: 'Box'
        , MODAL: 'Modal'
    }

    , attachEvent: function () {
        document.onkeydown = function (e) {
            e = e || window.event;
            if (e.keyCode == 27) {
                document.getElementById(Dialog.containerId).innerHTML = '';
            }
        };
    }

    , show: function (title, message, messageType, displayType = Dialog.DisplayType.BOX, containerId = 'dialogContainer') {

        Dialog.containerId = containerId;
        Dialog.attachEvent();

        switch (displayType) {
            case Dialog.DisplayType.BOX:
                Dialog.showBox(title, message, messageType);
                break;

            case Dialog.DisplayType.MODAL:
                Dialog.showModal(title, message, messageType);
                break;

            default:
                Dialog.showBox(title, message, messageType);
                break;
        }
    }

    , showBox: function (title, message, messageType) {

        var dialogContainer = document.getElementById(Dialog.containerId);
        dialogContainer.innerHTML = '';

        var dialog = document.createElement('div');
        dialog.setAttribute('id', 'mch-w3-custom-dialog');
        dialog.classList.add('w3-panel', 'w3-card-4');

        var buttonContainer = document.createElement('p');
        buttonContainer.setAttribute('id', 'buttonContainer');

        var btnOk = document.createElement('button');
        btnOk.setAttribute('id', 'btnOk');
        btnOk.setAttribute('type', 'button');
        btnOk.innerText = 'Ok';
        btnOk.classList.add('w3-button', 'w3-round');
        btnOk.addEventListener('click', function () {
            dialogContainer.innerHTML = '';
        });

        buttonContainer.appendChild(btnOk);
        buttonContainer.style.textAlign = 'right';

        var titleContainer = document.createElement('h3');
        titleContainer.setAttribute('id', 'titleContainer');
        titleContainer.innerText = title;

        var messageContainer = document.createElement('p');
        messageContainer.setAttribute('id', 'messageContainer');
        messageContainer.innerText = message;

        dialog.appendChild(titleContainer);
        dialog.appendChild(messageContainer);
        dialog.appendChild(buttonContainer);

        switch (messageType) {
            case Dialog.MessageType.SUCCESS:
                dialog.classList.add('w3-pale-green', 'w3-text-green');
                btnOk.classList.add('w3-green');
                break;

            case Dialog.MessageType.WARNING:
                dialog.classList.add('w3-pale-yellow', 'w3-text-brown');
                btnOk.classList.add('w3-yellow', 'w3-text-brown');
                break;

            case Dialog.MessageType.ERROR:
                dialog.classList.add('w3-pale-red', 'w3-text-red');
                btnOk.classList.add('w3-red');
                break;

            case Dialog.MessageType.INFORMATION:
                dialog.classList.add('w3-pale-blue', 'w3-text-blue');
                btnOk.classList.add('w3-blue');
                break;

            default:
                dialog.classList.add('w3-pale-blue');
                break;

        }

        dialogContainer.appendChild(dialog);
        btnOk.focus();
    }

    , showModal: function (title, message, messageType) {

        var dialogContainer = document.getElementById(Dialog.containerId);
        dialogContainer.innerHTML = '';

        var dialog = document.createElement('div');
        dialog.setAttribute('id', 'mch-w3-custom-dialog');
        dialog.classList.add('w3-modal');

        var contentDialog = document.createElement('div');
        contentDialog.setAttribute('id', 'contentDialog');
        contentDialog.classList.add('w3-modal-content', 'w3-animate-top', 'w3-card-4');

        var headerDialog = document.createElement('header');
        headerDialog.setAttribute('id', 'headerDialog');
        headerDialog.classList.add('w3-container');

        var titleHeader = document.createElement('h2');
        titleHeader.setAttribute('id', 'titleHeader');
        titleHeader.innerText = title;

        headerDialog.appendChild(titleHeader);

        var containerDialog = document.createElement('div');
        containerDialog.setAttribute('id', 'containerDialog');
        containerDialog.classList.add('w3-container');

        var messageContainer = document.createElement('p');
        messageContainer.setAttribute('id', 'messageContainer');
        messageContainer.innerText = message;

        var buttonContainer = document.createElement('p');
        buttonContainer.setAttribute('id', 'buttonContainer');

        var btnOk = document.createElement('button');
        btnOk.setAttribute('id', 'btnOk');
        btnOk.setAttribute('type', 'button');
        btnOk.innerText = 'Ok';
        btnOk.classList.add('w3-button', 'w3-round');
        btnOk.addEventListener('click', function () {
            dialog.style.display = 'none';
        });

        buttonContainer.appendChild(btnOk);
        buttonContainer.style.textAlign = 'right';

        containerDialog.appendChild(messageContainer);
        containerDialog.appendChild(buttonContainer);

        contentDialog.appendChild(headerDialog);
        contentDialog.appendChild(containerDialog);
        dialog.appendChild(contentDialog);

        switch (messageType) {
            case Dialog.MessageType.SUCCESS:
                headerDialog.classList.add('w3-green');
                btnOk.classList.add('w3-green');
                break;

            case Dialog.MessageType.WARNING:
                headerDialog.classList.add('w3-yellow');
                btnOk.classList.add('w3-yellow');
                break;

            case Dialog.MessageType.ERROR:
                headerDialog.classList.add('w3-red');
                btnOk.classList.add('w3-red');
                break;

            case Dialog.MessageType.INFORMATION:
                headerDialog.classList.add('w3-blue');
                btnOk.classList.add('w3-blue');
                break;

            default:
                headerDialog.classList.add('w3-blue');
                btnOk.classList.add('w3-blue');
                break;
        }

        dialogContainer.appendChild(dialog);
        dialog.style.display = 'block';
        btnOk.focus();
    }
};