'use strict';

class Resolve extends Signal {
    constructor() {
        super();
        const expires = $('input[type=hidden]#deadline').val();
        console.debug(expires, Date.now());
        this.timeout = setTimeout(() => (window.location.href = window.location.host), Date.now() - expires);
    }
}

const Page = new Resolve();

$(document).on('change', '#Resolution_Attachment', function () {
    $(this).parent().find('.js-filename').text($(this)[0].files[0].name);
});