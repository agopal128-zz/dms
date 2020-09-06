
"use strict";
define(['angularAMD', 'validationHelper'], function (angularAMD) {

    angularAMD.service('validationService', function () {

        var addErrorClass = function (ele, err) {
            if (!ele.hasClass('error')) {
                ele.addClass('error');
                ele.attr('title', err);
            }
        };

        this.validate = function (formId) {

            var form = $('#' + formId)[0],
                elementsToValidate = $('[data-validation-injector]', $('#' + formId)),
                errors = [],
                deferred = jQuery.Deferred();

            elementsToValidate.removeClass('error');

            if (form.checkValidity()) {
                deferred.resolve(true);
            }

            elementsToValidate.each(function () {
                var id = $(this).attr('id');

                //work around for empty space issue
                if ($(this).is('input:text') || $(this).is('textarea')) {
                    if ($(this).val().length > 0 && $.trim($(this).val()) === '') {
                        errors.push($(this).attr('data-' + validationType.required + '-message'));
                        addErrorClass($(this), $(this).attr('data-' + validationType.required + '-message'));
                    }
                }
                
                if (!form[id].checkValidity()) {

                    if ($(this).attr(validationType.required)) {
                        if (form[id].validity.valueMissing) {
                            errors.push($(this).attr('data-' + validationType.required + '-message'));
                            addErrorClass($(this), $(this).attr('data-' + validationType.required + '-message'));
                        }
                    }

                    if ($(this).attr(validationType.min) && form[id].validity.rangeUnderflow) {
                        errors.push($(this).attr('data-' + validationType.min + '-message'));
                        addErrorClass($(this), $(this).attr('data-' + validationType.min + '-message'));
                    }

                    if ($(this).attr(validationType.max) && form[id].validity.rangeOverflow) {
                        errors.push($(this).attr('data-' + validationType.max + '-message'));
                        addErrorClass($(this), $(this).attr('data-' + validationType.max + '-message'));
                    }

                    if ($(this).attr(validationType.maxLength) && form[id].validity.tooLong) {
                        errors.push($(this).attr('data-' + validationType.maxLength + '-message'));
                        addErrorClass($(this), $(this).attr('data-' + validationType.maxLength + '-message'));
                    }

                    if ($(this).attr(validationType.pattern) && form[id].validity.patternMismatch) {
                        errors.push($(this).attr('data-' + validationType.pattern + '-message'));
                        addErrorClass($(this), $(this).attr('data-' + validationType.pattern + '-message'));
                    }

                    if ($(this).attr(validationType.valid) && form[id].validity.typeMismatch) {
                        errors.push($(this).attr('data-' + validationType.valid + '-message'));
                        addErrorClass($(this), $(this).attr('data-' + validationType.valid + '-message'));
                    }
                    if ($(this).attr(validationType.valid) && form[id].validity.stepMismatch) {
                        errors.push($(this).attr('data-' + validationType.valid + '-message'));
                        addErrorClass($(this), $(this).attr('data-' + validationType.valid + '-message'));
                    }
                }

            });
            deferred.reject(errors);

            return deferred.promise();
        };
    });


});