import numeral from 'numeral'

export const removeNoNumeric = (val) => {
    if (val === undefined || val === null) return val;
    return val.toString().replace(/[^\d]+/g, '');
};

export const moneyInputFormat = (inputText) => {
    try {
        if (inputText === null || inputText === undefined || inputText === '')
            return '';
        let tmp = parseFloat(removeNoNumeric(inputText) + '');

        tmp = removeNoNumeric((tmp / 100).toFixed(2) + '');

        tmp = tmp.replace(/([0-9]{2})$/g, ',$1');
        if (tmp.length > 6)
            tmp = tmp.replace(/([0-9]{3}),([0-9]{2}$)/g, '.$1,$2');

        return tmp;
    } catch (ex) {
        return '';
    }
};

export const moneyInputFormatToFloat = (inputText) => {
    try {
        const valor = inputText.includes('-R$')
            ? inputText.replace('-R$', '') + ''
            : inputText.replace('R$', '') + '';
        const result = parseFloat(
            parseFloat(
                valor
                    .split('')
                    .filter(char => char != '.')
                    .join('')
                    .replace(',', '.'),
            ).toFixed(2),
        );
        return result;
    } catch (ex) {
        return 0;
    }
};

export const moneyLabel = (value: string | number) => {
    return numeral(value).format('$ 0,0.00');
};