
var sum = 0;
       var qt = 0;
       $('.item__price').each(function () {
           console.log($(this).text());

           var nextElement = $(this).next();

           sum += parseFloat($(this).text()) * nextElement.val();//Or this.innerHTML, this.innerText
    });

        $(".sum").text(sum.toFixed(2) + ' \u20AC');
        $(".taxes").text((sum * 0.23).toFixed(2));
        $(".total").text((sum + sum * 0.23).toFixed(2));