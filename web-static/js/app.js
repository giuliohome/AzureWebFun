function myFunction() {
    let xhr = new XMLHttpRequest();
	let username = document.getElementById("username").value;
	let url="https://giuliocloud.azurewebsites.net/api/HttpTrigger1?code=iVoNxPHGGeRXlvDe2lBxCFQiqyhgKejdVh0U9XcAnP9hOP9M7uDQrQ==&name=" + username;
	xhr.open("POST", url, true);
	xhr.onreadystatechange = function () {
		if (xhr.readyState === 4 && xhr.status === 200) {
			document.getElementById("cloudReply").innerHTML = this.responseText;
		}
	}
	xhr.send();
}
