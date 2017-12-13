//jQuery AJAX call postback on controller method FindTaxAnswer

$(document).ready(function () {
    //$('[data-toggle="popover"]').popover(); 

    $('#findtaxanswer').click(function () {

        var answerHtml = "<span class=\"glyphicon glyphicon-refresh spinning\"></span> Search...";
        $('#taxanswer').html(answerHtml);

        var answer;
        var score;
        var question = $('#taxquestion').val();  
		var matchingquestions;

        $.ajax({
            url: '/Home/FindTaxAnswer',
            dataType: "json",
            type: 'POST',
            data: JSON.stringify({ question: question }),
            processData: false,
            contentType: 'application/json; charset=utf-8',
            success: function (resp) {

				//Need to convert json string to a json obj to be able to pull out the attributes
				var ob = JSON.parse(resp);
                answer = ob.answers[0].answer;
				score = ob.answers[0].score;
				matchingquestions = ob.answers[0].questions;
                answer = htmlDecode(answer);

                answerHtml = "<div>" + answer + "</div><br /><div><hr><br /><hr><h4>Score:</h4>" + score + "</div><br /><div><h4>Matching Questions:</h4>" + matchingquestions + "</div>";

                $('#taxanswer').html(answerHtml);

            }
        });
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


function htmlDecode(input) {
    var temp = document.createElement('div');
    temp.innerHTML = input;
    return temp.childNodes[0].nodeValue;
}
