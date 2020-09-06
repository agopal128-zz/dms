'use strict';
define(['angularAMD'], function (angularAMD) {

    angularAMD.filter('metricType', ['$filter', function ($filter) {
        return function (input, dataType, isShowingActualPopupOrTarget) {

            var unit = '';
            var value = input;

            // append appropriate units  if the number is too big to display in circle.
            var appendUnit = function (input) {
                switch (true) {
                    case Math.abs(input) >= 1000000000:
                        input = formatInput(input / 1000000000);
                        unit = 'B';
                        break;
                    case Math.abs(input) >= 1000000:
                        input = formatInput(input / 1000000);
                        unit = 'M';
                        break;
                    case Math.abs(input) < 1000000 && Math.abs(input) >= 1000:
                        input = formatInput(input / 1000);
                        unit = 'k';
                        break;
                    case Math.abs(input) < 1000:
                        input = input;
                        unit = '';
                        break;
                }
                return input;
            };
            
            var appendSymbolAccordingToDatatype = function (dataType) {
                switch (dataType) {
                    case 0: //Whole Number

                    case 1: //Decimal Number
                        input = input + unit;
                        break;

                    case 2: //Percentage
                        input = input + unit + "%";
                        break;

                    case 3: //Amount
                        input = input < 0 ? "-$" + (input * -1) + unit : "$" + input + unit;
                        break;
                }
            };

            var formatInput = function (input) {
                //format input to maximum of 4 characters (max 6 characters including unit and symbol)
                //999.999 changes to 999
                //99.999 changes to 99.9
                input = Math.abs(input) >= 100 || input <= -10 ? Math.floor(input) : Math.floor(input * 10) / 10;
                return input;
            };

            if ((input && input !== '') || input === 0) {
                if (!isShowingActualPopupOrTarget) {
                    input = appendUnit(input);
                }

                appendSymbolAccordingToDatatype(dataType);
                
                if (!isShowingActualPopupOrTarget && Math.abs(value) < 1000 && dataType != 2) {
                    //format to maximum of 5 characters
                    input = input.toString().charAt(4) === '.' ? input.toString().slice(0, 4) : input.toString().slice(0, 5);
                }
            }
            return input;
        };
    }]);
});
