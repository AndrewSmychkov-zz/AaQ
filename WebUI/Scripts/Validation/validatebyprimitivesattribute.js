jQuery.validator.addMethod("validatebyprimitives", function (value, element, param) {

    try {
        var interfacenameString = param["nameofinterface"];
        var propertyString = param["nameofproperty"];

        switch (interfacenameString) {
            case "IUserModel":
                switch (propertyString) {
                    case "Login":
                        return isregex(value, "^.{1,50}$");
                    case "Password":
                        return isregex(value, "^.{2,}$");
                }
                break;
            case "IAnswerModel":
                switch (propertyString) {
                    case "Name":
                        return isregex(value, "^.{1,250}$");
                }
                break;
            case "IPackageModel":
                switch (propertyString) {
                    case "Name":
                        return isregex(value, "^.{1,250}$");
                }
                break;
            case "IQuestionModel":
                switch (propertyString) {
                    case "Text":
                        return isregex(value, "^.{1,150}$");
                }
                break;
        }

    } catch (e) {
        window.SetExeptionLog("Валидатор validatebyprimitives: " + e.message);
        return false;
    }
});


jQuery.validator.unobtrusive.adapters.add("validatebyprimitives", ["nameofproperty", "nameofinterface"], function (options) {
    options.rules["validatebyprimitives"] =
    {
        nameofproperty: options.params.nameofproperty,
        nameofinterface: options.params.nameofinterface
    };
    options.messages["validatebyprimitives"] = options.message;
});

function isregex(value, regex) {
    var re = new RegExp(regex);
    var aa = re.test(value);
    return aa;
}