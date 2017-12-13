//jQuery AJAX call postback on controller method FindTaxAnswer

$(document).ready(function () {

    $('#taxquestion').keypress(function (event) {
        if (event.which == 13) {

            $('#findtaxanswer').click();
        }
    });


    $('.faq').click(function () {
        if ($(this).next('.answer').css('display') == 'none') {
            $('.answer').hide();
            $(this).next('.answer').show();
        } else {
            $('.answer').hide();
        }
    });
});
