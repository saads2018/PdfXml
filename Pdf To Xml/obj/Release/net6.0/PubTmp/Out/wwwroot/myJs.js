function saveAsFile(fileName, bytesBase64, backToLink) {

    var link = document.createElement('a');

    link.download = fileName;

    link.href = 'data:application/pdf;base64,' + bytesBase64;

    document.body.appendChild(link);

    link.click();

    document.body.removeChild(link);

    var link = document.createElement('a');

    link.href = backToLink;

    document.body.appendChild(link);

    link.click();

    document.body.removeChild(link);
}

function reload(linkLocation) {

    var link = document.createElement('a');

    link.href = linkLocation;

    document.body.appendChild(link);

    link.click();

    document.body.removeChild(link);

}

function resetSelectList(count,value) {

    for (let i = 0; i < count; i++) {
        document.getElementById("mySelect " + i).value = value;   
    }
}

function getAlert(message)
{
    alert(message);
}

