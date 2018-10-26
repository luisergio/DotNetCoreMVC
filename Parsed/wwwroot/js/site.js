
$(document).ready(function () {

    //Lógica para troca da lingua selecionada
    $(".SetLanguage").click(function (e) {
        e.preventDefault();
        $.ajax({
            url: $(this).attr("href"), 
            success: function () {
                console.log("Language setted!");
                document.location.reload();
            }
        });
    });

});