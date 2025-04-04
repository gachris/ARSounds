function updateFileName() {
  var label = document.getElementById('custom-file-label');
  var input = document.getElementById('file');

  if (input.files[0] != null && input.files[0] != undefined) {
    label.innerText = input.files[0].name;
  } else {
    label.innerText = label.getAttribute('value');
  }
}

window.addEventListener("load", function () { document.querySelector('body').classList.add('loaded'); });
