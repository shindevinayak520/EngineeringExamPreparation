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

//function ChangeStatus(event) {
//    event.preventDefault();
//    if (event.val() == "submitted") {

//    }

//}


$(document).ready(function () {

    $(".question").not("#question-1").hide();
    $(".question-number").click(function (e) {
        var questionId = $(this).data("question-id");

        console.log("Question ID:", "#" + questionId);
        //$("#" + questionId).removeClass("d-none");
        $(".question").not("#" + questionId).hide();
        $("#" + questionId).show();
    });

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
        $("#" + currentQuestion).hide().removeClass("bg-white");
        nextQuestion.show();
 
        // Mark the number of the next question as red
        var nextQuestionNumber = nextQuestion.data('question-id');
        if (nextQuestion.hasClass("notVisit")) {
            $('.question-number[data-question-id="' + nextQuestionNumber + '"]').removeClass('bg-white').addClass('bg-danger text-white');
        }
    }

}


//function changeBackground(clickedElement) {

//    if (clickedElement.classList.contains('bg-white')) {

//        $(".question-number").not(clickedElement).not('.bg-info').not('.bg-success').removeClass("bg-danger text-white").addClass("bg-white bg-gradient");
//        $(clickedElement).addClass("bg-danger text-white").removeClass("bg-white");
//    }

//    if (clickedElement.classList.contains('bg-info')) {
//        $(".question-number").not(clickedElement).not('.bg-info').not('.bg-success').removeClass("bg-danger text-white").addClass("bg-white bg-gradient");
//        //$(".question-number").not(clickedElement).not(":not(.bg-info)").removeClass("bg-danger text-white").addClass("bg-white bg-gradient");
//        //$(clickedElement).addClass("bg-danger text-white").removeClass("bg-white");
//    }
//}

$(document).ready(function () {
    // Initialize question statuses based on UI classes
    $(".question").each(function () {
        updateQuestionStatus($(this));
    });

    // Event listener for marking review
    $(".markReviewBtn").click(function (e) {
        e.preventDefault();
        var questionId = $(this).data("question-id");
        var questionElement = $("#" + questionId);
        toggleQuestionStatus(questionElement, "MarkedForReview");
        showNextQuestion(questionId);
    });

    // Event listener for saving answer
    $(".ansSaved").click(function (e) {
        e.preventDefault();
        var questionId = $(this).data("question-id");
        var questionElement = $("#" + questionId);
        // Check if any radio input in the same question is selected
        var anySelected = $(this).closest('#' + questionId).find('input[type="radio"]:checked').length > 0;

        if (anySelected) {
            toggleQuestionStatus(questionElement, "Answered");
            showNextQuestion(questionId);
        }
    });


});

function toggleQuestionStatus(questionElement, status) {
    if (questionElement.hasClass("notVisit")) {
        questionElement.removeClass("notVisit").addClass(status);
    } else if (questionElement.hasClass("MarkedForReview") || questionElement.hasClass("Answered")) {
        questionElement.removeClass("MarkedForReview Answered").addClass(status);
    } else if (questionElement.hasClass("AnsweredAndMarkedForReview")) {
        questionElement.removeClass("AnsweredAndMarkedForReview").addClass(status);
    }
    updateQuestionStatus(questionElement);
}

function updateQuestionStatus(questionElement) {
    var questionId = questionElement.attr("id");
    var status = "NotVisited";
    if (questionElement.hasClass("NotAnswered")) {
        status = "NotAnswered";
    } else if (questionElement.hasClass("MarkedForReview")) {
        status = "MarkedForReview";
    } else if (questionElement.hasClass("Answered")) {
        status = "Answered";
    } else if (questionElement.hasClass("AnsweredAndMarkedForReview")) {
        status = "AnsweredAndMarkedForReview";
    }
    // Update question status in UI (you can implement this as needed)
    updateQuestionOverview(questionId, status);
}

function updateQuestionOverview(questionId, status) {
    var overviewItem = $(".question-number[data-question-id='" + questionId + "']");
    var countElement = $("#" + status + "Count");
   
    // Increment/decrement the count based on status
    var count = parseInt(countElement.text());

    if (overviewItem.length > 0) {

        // Get the previous status of the question
        var prevStatus = "";
        if (overviewItem.hasClass("notVisit") || overviewItem.hasClass("bg-white")) {
            prevStatus = "NotVisited";
        } else if (overviewItem.hasClass("bg-success")) {
            prevStatus = "Answered";
        } else if (overviewItem.hasClass("bg-danger")) {
            prevStatus = "NotAnswered";
        } else if (overviewItem.hasClass("bg-info")) {
            prevStatus = "MarkedForReview";
        } else if (overviewItem.hasClass("bg-primary")) {
            prevStatus = "AnsweredAndMarkedForReview";
        }


        // Remove the old status class
        overviewItem.toggleClass("bg-success bg-danger bg-info bg-primary notVisit bg-white", false);

        // Add the new status class
        if (status === "NotVisited") {
            overviewItem.addClass("bg-white");
        } else if (status === "Answered") {
            overviewItem.addClass("bg-success");
        } else if (status === "NotAnswered") {
            overviewItem.addClass("bg-danger");
        } else if (status === "MarkedForReview") {
            overviewItem.addClass("bg-info");
        } else if (status === "AnsweredAndMarkedForReview") {
            overviewItem.addClass("bg-primary");
        }

        // Update count
        countElement.text(count + 1); // Increment count

        // Update count for the previous status
        var prevCountElement = $("#" + prevStatus + "Count");
        var prevCount = parseInt(prevCountElement.text());

        if (prevCount > 0) {
            prevCountElement.text(prevCount - 1); // Decrement count
        }
        
    }
}

function changeBackground(clickedElement) {
    var questionId = $(clickedElement).data("question-id");
    var questionElement = $("#" + questionId);

    // Check if the question was previously answered or marked for review
    var prevStatus = "";
    if (questionElement.hasClass("bg-success")) {
        prevStatus = "Answered";
    } else if (questionElement.hasClass("bg-info")) {
        prevStatus = "MarkedForReview";
    } else if (questionElement.hasClass("bg-primary")) {
        prevStatus = "AnsweredAndMarkedForReview";
    }

    // Update the question overview and counts
    updateQuestionOverview(questionId, "NotAnswered");
}




