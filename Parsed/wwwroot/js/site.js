// Write your JavaScript code.


$(".SetLanguage").click(function (e) {

    e.preventDefault();
    $.ajax({

        url: $(this).attr("href"), // comma here instead of semicolon   
        success: function () {
            alert("Value Added");  // or any other indication if you want to show
        }

    });

});
