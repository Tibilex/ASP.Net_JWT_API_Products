var tokenKey = "accessToken";

document.addEventListener('DOMContentLoaded', () =>{

    $("#signup").click(()=>{
        let check = $("#typeCheckBox:checked").is(":checked");
        if(check !== true){
            Autorisation();
        }
        else{
            Registration();
        }
    });
    
    $("#typeCheckBox").change(()=>{
        SelectForm();
    });
    
    $("#addProduct").click(()=>{
        AddProduct();
        alert("123")
    });  
})

function SelectForm(){
    let check = $("#typeCheckBox:checked").is(":checked");
    if(check !== true){
        $("h3").text("Login");
        $(".sign-in__button").text("Login");
        $(".login__text").text("Don't have an account?");
    }
    else{
        $("h3").text("registration");
        $(".sign-in__button").text("Registration");
        $(".login__text").text("Have already an account?");
    }
}

function Autorisation(){
    let password = $("#pass").val();
 
    $.post("/api/Authentication/Login", {
        usermail: $("#mail").val(),
        password: password
    })
    .done(function(response) {
        document.location = "/html/adminPage.html";
        sessionStorage.setItem(tokenKey, response.token);
    })
    .fail((response) =>{
        alert(response.status);
    });
}

function Registration(){
    let password = $("#pass").val();

    $.post("/api/Authentication/Registration", {
        usermail: $("#mail").val(),
        password: password,
    })
    .done(function(response) {
        console.log(response);
        alert("Registration successful!");
    })
    .fail((response) =>{
        alert(response.status);
    });  
}

async function AddProduct(){
    const token = sessionStorage.getItem(tokenKey);
    await $.ajax({
        url: '/api/CreateProduct',
        type: 'POST',
        data: {
            productId: 0,
            productName: $("#productName").val(),
            productDescription: $("#productDescription").val(),
            productCost: $("#productCost").val(),
            productStock: $("#productStock").val(),
            //productImageUrl: $("#productImageUrl").val(),
        },
        headers: {
            "Accept": "application/json",
            "Authorization": "Bearer " + token
        },
        success: function (data){
            console.log(data);
            AdminGetProducts();
        },
        error: function (data){
            console.error(data);  
        }
    });
}
