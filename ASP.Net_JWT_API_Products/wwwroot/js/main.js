
document.addEventListener('DOMContentLoaded', () =>{

    $(".sign-in__button").click(()=>{
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
    
    $(".close__form").click(()=>{
        
    });
})

function SelectForm(){
    let check = $("#typeCheckBox:checked").is(":checked");
    if(check !== true){
        $("h3").text("Login");
        $(".sign-in__button").text("sign in");
        $(".login__text").text("Don't have an account?");
    }
    else{
        $("h3").text("registration");
        $(".sign-in__button").text("sign up");
        $(".login__text").text("Have already an account?");
    }
}

function Autorisation(){
    let password = $("#pass").val();
    if(Validate(password)){
        $.post("/api/Authentication/login", {
            email: $("#mail").val(),
            password: password
        })
        .done(function(response) {
            Login();
            sessionStorage.setItem(tokenKey, response.token);
        })
        .fail((response) =>{
            alert(response.status);
        });
    }
}
