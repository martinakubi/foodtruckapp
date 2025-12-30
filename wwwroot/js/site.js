// Print functions

function numForPrinter(number, decimals = 0) {
    return number.toFixed(decimals);
}

function generateReceiptText(printer, orderItems, orderId, orderDate, numberOnly, kennitala1, merchantAddress,
    merchantPhone, merchantEmail, merchantKennitala, merchantVsk) 
{
    
    printer.addTextAlign(printer.ALIGN_CENTER);
    printer.addTextSize(2, 2);
    printer.addText(`We'll call number: ${numberOnly}\n`);
    printer.addText("------------------------\n");
    //Receipt
    printer.addText(`${merchantName}\n`);
    printer.addTextSize(1, 1);
    printer.addText(`${merchantAddress}\n`);
    printer.addText(`Sími: ${merchantPhone}\n`);
    printer.addText(`Netfang: ${merchantEmail}\n`);
    printer.addText(`Kennitala: ${merchantKennitala}\n`);
    printer.addText(`VSK númer: ${merchantVsk}\n`);
    printer.addText("------------------------------------------------\n");
    printer.addTextSize(1, 1);
    printer.addText(`Kvittun nr: ${orderId}\n`);
    printer.addText(`Dagsetning: ${orderDate}\n`);
    if (kennitala1)
    {
    printer.addText(`Kennitala: ${kennitala1}\n`);
    }
    printer.addText("------------------------------------------------\n");
    printer.addTextAlign(printer.ALIGN_LEFT);
    let total = 0;
    let taxRate = 0.11;
    let netTotal = 0;
    let taxAmount = 0;
    orderItems.forEach(item => {
        const name = item.Name;
        const qty = item.NumOfPortions;
        const pricePerOne = item.PricePerOne;
        const subtotal = qty * pricePerOne;
        total += subtotal;
        function formatTwoColumn(left, right, width = 48) {
            const space = width - left.length - right.length;
            return left + ' '.repeat(space > 0 ? space : 1) + right;
        }
        const line = formatTwoColumn(`${name} ${qty}x`, `${numForPrinter(pricePerOne)} ISK`);
        console.log(line);
        printer.addText(line + "\n");
        printer.addTextAlign(printer.ALIGN_LEFT);
    });
    printer.addText("------------------------------------------------\n");
    netTotal = Math.round(total / (1 + taxRate));
    taxAmount = total - netTotal;
    printer.addTextAlign(printer.ALIGN_RIGHT);
    printer.addText(`VSK grunnur (11%): ${numForPrinter(netTotal)} ISK\n`);
    printer.addText(`VSK (11%): ${numForPrinter(taxAmount)} ISK\n`);
    printer.addText(`Samtals: ${numForPrinter(total)} ISK\n\n`);
    printer.addTextAlign(printer.ALIGN_CENTER);
    printer.addText("Takk fyrir viðskiptin!\n\n");
    printer.addCut(printer.CUT_FEED);

    //Cooker list
    printer.addTextSize(1, 1);
    printer.addText(`\n\n\n`);
    printer.addText(`Invoice number: ${orderId}\n`);
    printer.addText(`Invoice date: ${orderDate}\n`);
    printer.addTextAlign(printer.ALIGN_LEFT);

    orderItems.forEach(item => {
        const name = item.Name;
        const qty = item.NumOfPortions;

        function formatTwoColumn(left, right, width = 24) {
            if(left.length > 22) {
                left = left.substring(0, 22);
            }
            const space = width - left.length - right.length;
            return left + ' '.repeat(space > 0 ? space : 1) + right;
        }

        const line = formatTwoColumn(`${name}`, `${qty}`);
        printer.addTextSize(2, 2);
        printer.addText(line + "\n");
        printer.addTextSize(1, 1);
        printer.addText("-------------------\n");
    });
    printer.addTextAlign(printer.ALIGN_CENTER);
    printer.addTextSize(2, 2);
    printer.addText(`Order ${numberOnly}\n`);
    printer.addText("\n");
    printer.addCut(printer.CUT_FEED);
}
