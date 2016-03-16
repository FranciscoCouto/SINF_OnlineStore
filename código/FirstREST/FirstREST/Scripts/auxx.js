function Increase(nome)
{
    var y = document.getElementById(nome).innerHTML;
    document.getElementById(nome).innerHTML = Number(y) + 1;

    var hidden = document.getElementById(nome + " + price + Quantidade");
    hidden.value = Number(y) + 1;
    var hQuantidade = document.getElementById("hidden " + nome);
    hQuantidade.value = hidden.value;

    var sum = 0;
    var qt = 0;
    $('.item__price').each(function () {
        console.log($(this).text());

        var nextElement = $(this).next();

        sum += parseFloat($(this).text()) * nextElement.val();  //Or this.innerHTML, this.innerText
    });

    $(".sum").text(sum.toFixed(2));
    $(".taxes").text((sum * 0.23).toFixed(2));
    $(".total").text((sum + sum * 0.23).toFixed(2));
}

function Decrease(nome) {
    var y = document.getElementById(nome).innerHTML;
    document.getElementById(nome).innerHTML = Number(y) - 1;

    var hidden = document.getElementById(nome + " + price + Quantidade");
    hidden.value = Number(y) - 1;
    var hQuantidade = document.getElementById("hidden " + nome);
    hQuantidade.value = hidden.value;
    var sum = 0;
    var qt = 0;
    $('.item__price').each(function () {
        console.log($(this).text());

        var nextElement = $(this).next();

        sum += parseFloat($(this).text()) * nextElement.val();  //Or this.innerHTML, this.innerText
    });

    $(".sum").text(sum.toFixed(2));
    $(".taxes").text((sum * 0.23).toFixed(2));
    $(".total").text((sum + sum * 0.23).toFixed(2));
}