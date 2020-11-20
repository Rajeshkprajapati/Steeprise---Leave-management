let multiselector = {};
multiselector = (function () {
    let defaultOptions = {
        isDisableSelector: false,
        includeSelectAllOption: false,
        includeSelectAllIfMoreThan: 0,
        selectAllText: ' Select all',
        allSelectedText: 'All selected',
        maxSelection: -1,
        buttonWidth: "100%",
        maxHeight: "250",
        enableFiltering: true,
        enableCaseInsensitiveFiltering: true,
        nonSelectedText: '-Select-',
        buttonClass: 'btn btn-default multiselector'
    };
    let initSelector = function (selector, options, selectedValuesStore, selectedValueSeparator) {
        if ((selector && selector.length > 0) && (selectedValuesStore && selectedValuesStore.length > 0)) {
            selectedValueSeparator = selectedValueSeparator || ",";
            options.onChange = function (opt, chk) {
                let id = opt.val();
                let selIds = selectedValuesStore.val().split(selectedValueSeparator).filter(r => r !== "");
                if (chk) {
                    selIds.push(id);
                }
                else {
                    let i = selIds.findIndex(function (item) {
                        return item === id;
                    });
                    selIds.splice(i, 1);
                }
                selectedValuesStore.val(selIds.join(selectedValueSeparator));
                manageOptionsSelection(selector, options, selIds);
            };
            options= simplifyOptions(defaultOptions, options);
            $(selector).multiselect(options);
            displaySelectedValues(selector, selectedValuesStore, selectedValueSeparator);

            if (options.isDisableSelector) {
                disableSelector(selector);
            }
        }
    };

    let manageOptionsSelection = function (selector, options, selIds) {
        if (options.maxSelection > 0 && selIds.length === options.maxSelection) {
            disableUnselectedItems(selector, selIds);
        }
        else {
            if (!options.isDisableSelector) {
                enableSelector(selector);
            }
        }
    };

    let displaySelectedValues = function (selector, selectedValuesStore, selectedValueSeparator) {
        let values = selectedValuesStore.val();
        if (values) {
            values = values.split(selectedValueSeparator).map(r => r.trim()).filter(r => r !== "");
            selector.multiselect("select", values);
        }
    };

    let disableSelector = function (selector) {
        $(selector).find('option').each(function (i, o) {
            let input = $('input[value="' + $(o).val() + '"]');
            input.closest("li").eq(0).addClass("disable-click");
        });
    };

    let simplifyOptions = function (defaultOptions, customOptions) {
        for (let key in defaultOptions) {
            let value = customOptions[key];
            customOptions[key] = value ? value : defaultOptions[key];
        }
        return customOptions;
    };

    let enableSelector = function (selector) {
        $(selector).find('option').each(function (i, o) {
            let input = $('input[value="' + $(o).val() + '"]');
            input.closest("li").eq(0).removeClass("disable-click");
        });
    };

    let disableUnselectedItems = function (selector, selIds) {
        $(selector).find('option').each(function (i, o) {
            let val = $(o).val();
            if (selIds.indexOf(val) < 0) {
                let input = $('input[value="' + val + '"]');
                input.closest("li").eq(0).addClass("disable-click");
            }
        });
    };

    return {
        initSelector: initSelector
    };
})();