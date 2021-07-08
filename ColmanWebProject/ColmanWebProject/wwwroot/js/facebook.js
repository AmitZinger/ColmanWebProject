$(document).ready(function () {
    FB.init({
        appId: '827965678096965',
        autoLogAppEvents: true,
        xfbml: true,
        version: 'v11.0'
    });

    $('#shareBtn').click(function () {
        FB.ui({
            display: 'popup',
            method: 'share',
            href: $('#imageToShare').attr('src'),
        }, function (response) { });
    })
});
