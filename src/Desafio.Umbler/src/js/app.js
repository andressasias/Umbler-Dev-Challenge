const Request = window.Request
const Headers = window.Headers
const fetch = window.fetch

class Api {
    async request(method, url, body) {
        if (body) {
            body = JSON.stringify(body)
        }

        const request = new Request('/api/' + url, {
            method: method,
            body: body,
            credentials: 'same-origin',
            headers: new Headers({
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            })
        })

        const resp = await fetch(request)
        if (!resp.ok && resp.status !== 400) {
            throw Error(resp.statusText)
        }

        const jsonResult = await resp.json()

        if (resp.status === 400) {
            if (jsonResult == "Non-Existent Domain") {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: "Dom\u00ednio n\u00e3o encontrado",
                })
            }
            jsonResult.requestStatus = 400
        }

        if (resp.status === 500) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: "Ocorreu um erro. Mensagem:" + jsonResult ,
                })
            jsonResult.requestStatus = 500
        }

        return jsonResult
    }

    async getDomain(domainOrIp) {
        return this.request('GET', `domain/${domainOrIp}`)
    }
}

const api = new Api()

//função que irá se comunicar com o Domain Controller, receber o domínio e ajustar para apresentar na tela
async function searchDomain() {

    var error = checkForm();
    if (error == false) {
        return false;
    } else {
        const txt = document.getElementById('txt-search')
        const divName = document.getElementById('name')
        const divWhois = document.getElementById('whois')

        console.log(txt.value);
        const response = await api.getDomain(txt.value)
        if (response) {
            var whois = response.whoIs.split("\n");
            var infoWhois = {};
            whois.forEach(keyValue => {
                var partes = keyValue.split(": ");
                var key = partes.shift();
                var value = partes.join(": ");
                if (infoWhois[key] == undefined) {
                    infoWhois[key] = value;
                } else if (infoWhois[key] instanceof Array) {
                    infoWhois[key].push(value);
                } else {
                    infoWhois[key] = [infoWhois[key], value];
                }
            });

            var listWhois = "";

            for (var property in infoWhois) {
                if (infoWhois[property] != "" & property != "") {
                    listWhois += `<li>${property}: ${infoWhois[property] instanceof Array ? infoWhois[property].join("<br>") : infoWhois[property]}</li>`;
                }
            }
            divName.innerHTML = response.name;
            divWhois.innerHTML = `<li> Ip:${response.ip}</li><li>Hosted At: ${response.hostedAt}</li>${listWhois}`;
            document.getElementById("cards-results").style.display = "block";
        }
    }
}

//função para checkar se domínio foi preenchido corretamente
function checkForm() {

    var dominio = document.getElementById("txt-search").value.toString();
    if ((dominio == "") |
        (dominio.indexOf(".") == -1) |
        (dominio.indexOf(".") == dominio.length - 1) |
        (dominio.indexOf(" ") >= 0) |
        (dominio.indexOf(".") == 0)) {
        Swal.fire({
            icon: 'info',
            text: 'Por favor, prencher com um dom\u00ednio completo. Exemplo: umbler.com',
            showConfirmButton: false,
            timer: 2700
        })
        document.getElementById("txt-search").focus();
        return false
    }
}

var callback = () => {

    document.getElementById('btn-search').addEventListener('click', () => {
        searchDomain();
    });
    
    document.getElementById("txt-search").addEventListener("focus", function () {
        document.getElementById("txt-search").addEventListener('keydown', function (event) {
            if (event.keyCode == 13) {
                searchDomain();
            }
        });
    });
}

if (document.readyState === 'complete' || (document.readyState !== 'loading' && !document.documentElement.doScroll)) {
    callback()
} else {
    document.addEventListener('DOMContentLoaded', callback)
}
