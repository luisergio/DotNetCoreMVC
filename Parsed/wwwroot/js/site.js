
$(document).ready(function () {

    //L�gica para troca do idioma selecionado
    $(".SetLanguage").click(function (e) {
        e.preventDefault();
        $.ajax({
            url: $(this).attr("href"), 
            success: function () {
                document.location.reload();
            }
        });
    });

});