// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function getCharName() {
    let charName = document.getElementById("nameField").innerHTML();
    let x = "pp"
    $.ajax({
        url: '/Home/Test',
        type: 'POST',
        data: { name: x },
        success: function (response) {
            console.log(response); // Output the response in the console
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error(error);
        }
    });
}