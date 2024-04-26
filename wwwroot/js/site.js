// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('.dropdown').hover(function () {
        $(this).addClass('show');
        $(this).find('.dropdown-menu').addClass('show');
    }, function () {
        $(this).removeClass('show');
        $(this).find('.dropdown-menu').removeClass('show');
    });
});

function ChangeStatus(event) {
    event.preventDefault();
    if (event.val() == "submitted") {

    }

}


$(document).ready(function () {
    // Event handler for clicking on question numbers
    var markedQuestions = {};
    var answeredQuestions = {};
    var notVisitedQuestions = {};
    var markForReviewCount = 0;
    var notVisitedCount = 0;
    var answeredCount = 0;
    var notAnsweredCount = 0;

    

    $(".question").not("#question-1").hide();
    $(".question-number").click(function (e) {
        var questionId = $(this).data("question-id");

        console.log("Question ID:", "#" + questionId);
        //$("#" + questionId).removeClass("d-none");
        $(".question").not("#" + questionId).hide();
        $("#" + questionId).show();
    });

    $(".markReviewBtn").click(function (e) {
        e.preventDefault();
        var questionId = $(this).data("question-id");

        console.log("Question ID Marked for review : ", questionId);
        //$("#" + questionId).removeClass("d-none");
        $("." + questionId).removeClass("bg-white text-white bg-danger text-white");
        $("." + questionId).addClass("bg-info text-white");
        $("#" + questionId).addClass("mark-for-review").removeClass("notVisit");
        $("#" + questionId).show();
        var markForReview = updateMarkForReview(questionId);
        markForReviewCount = markForReview;
        $('#markForReviewCount').text(markForReviewCount); // Update the content of the element

        var notVisit = updateNotVisited(questionId);
        notVisitedCount = notVisit;
        $('.notVisitedCount').text(notVisitedCount);
        
    });

    $(".ansSaved").click(function (e) {
        e.preventDefault();
        var questionId = $(this).data("question-id");

        // Check if any radio input in the same question is selected
        var anySelected = $(this).closest('#'+questionId).find('input[type="radio"]:checked').length > 0;

        if (anySelected) {
            console.log("Question ID submitted : ", questionId);
            //$("#" + questionId).removeClass("d-none");
            $("." + questionId).removeClass("bg-white text-white bg-danger bg-info");
            $("." + questionId).addClass("bg-success text-white");
            $("#" + questionId).removeClass("mark-for-review").addClass("saved").removeClass("notVisit");
            showNextQuestion(questionId);
            var markForSaved = updateSaved(questionId);
            answeredCount = markForSaved;
            $('#markForSavedCount').text(answeredCount); // Update the content of the element

            var notVisit = updateNotVisited(questionId);
            notVisitedCount = notVisit;
            $('.notVisitedCount').text(notVisitedCount);


        }
        else {
            alert("Please select a option then save or else marked as review");
        }
        
    });

    function updateMarkForReview(question) {
        var markForReview = markForReviewCount;
        $('.question').each(function () {
            if (!Object.keys(markedQuestions).includes(question)) {
                if ($(this).hasClass('mark-for-review')) {
                    markedQuestions[question] = true;
                    markForReview++;
                }
            }
            
            
        });
        return markForReview;
    }

    function updateSaved(question) {
        var saved = answeredCount;

        $('.question').each(function () {
            if (!Object.keys(markedQuestions).includes(question)) {
                if ($(this).hasClass('saved')) {
                    answeredQuestions[question] = true;
                    saved++;
                }
            }
        });
        return saved;
    }

    function updateNotVisited(question) {
        var notVisited = notVisitedCount;

        $('.question').each(function () {
            if (!Object.keys(markedQuestions).includes(question)) {
                if ($(this).hasClass('notVisit')) {
                    notVisitedQuestions[question] = true;
                    notVisited++;
                }
            }
        });
        return notVisited;
    }



});

//$('.question').click(function (event) {
//    var submitForm = confirm('Are you sure you want to proceed to add subjects?');
//    if (!submitForm) {
//        event.preventDefault(); // Prevent form submission
//    }
//});

function showNextQuestion(currentQuestion) {
    var nextQuestion = $("#" + currentQuestion).next('.question');

    if (nextQuestion.length > 0) {
        // Show the next question
        $("#" + currentQuestion).hide();
        nextQuestion.show();
 
        // Mark the number of the next question as red
        var nextQuestionNumber = nextQuestion.data('question-id');
        $('.question-number[data-question-id="' + nextQuestionNumber + '"]').removeClass('bg-white').addClass('bg-danger text-white');
    }

}


function changeBackground(clickedElement) {

    if (clickedElement.classList.contains('bg-white')) {

        $(".question-number").not(clickedElement).not('.bg-info').not('.bg-success').removeClass("bg-danger text-white").addClass("bg-white bg-gradient");
        $(clickedElement).addClass("bg-danger text-white").removeClass("bg-white");
    }

    if (clickedElement.classList.contains('bg-info')) {
        $(".question-number").not(clickedElement).not('.bg-info').not('.bg-success').removeClass("bg-danger text-white").addClass("bg-white bg-gradient");
        //$(".question-number").not(clickedElement).not(":not(.bg-info)").removeClass("bg-danger text-white").addClass("bg-white bg-gradient");
        //$(clickedElement).addClass("bg-danger text-white").removeClass("bg-white");
    }
}

