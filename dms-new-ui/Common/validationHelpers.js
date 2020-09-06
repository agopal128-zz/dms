var ValidationRule = function (type, value, message) {
    this.type = type;
    this.value = value;
    this.message = message;
};

var validationType = {
    required: 'required',
    max: 'max',
    min: 'min',
    maxLength: 'maxlength',
    pattern: 'pattern',
    valid: 'valid'
};