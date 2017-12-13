//jQuery AJAX call postback on controller method FindTaxAnswer

$(document).ready(function () {

    $('#findtaxanswer').click(function () {

        //TODO: write ajax function to get response which will need to be decoded and then shown in a modal
       
    });


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
