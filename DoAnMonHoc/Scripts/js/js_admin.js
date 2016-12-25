var text = tinyMCE.get('MoTaCT').getContent
function getText()
{
    var content = "", body = tinyMCE.activeEditor.getDoc().body;

    if (body.textContent) content = body.textContent; // Mozilla (Gecko)
    if (body.innerText) content = body.innerText; // Internet Explorer
    if (content == "") content = tinyMCE.activeEditor.getContent(); // other browsers

    content = content.replace(/<\/?[^>] +>/g, "");
    alert(content);
}