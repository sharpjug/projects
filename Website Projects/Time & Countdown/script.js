//const targetDate = new Date('YYYY-MM-DDTHH:MM:SS'); // Set your target
//const targetDate = new Date('2024-05-25 00:00:01')

function updateCountdown() {
  const currentTime = new Date();
  var targetDate = new Date(document.getElementById("dateTo").value);
  const difference = targetDate - currentTime;



  const days = Math.floor(difference / (1000 * 60 * 60 * 24));
  const hours = Math.floor((difference % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
  const minutes = Math.floor((difference % (1000 * 60 * 60)) / (1000 * 60));
  const seconds = Math.floor((difference % (1000 * 60)) / 1000);

  if (document.getElementById("dateTo").value == "" | isNaN(difference)) {
    document.getElementById("days").innerText = "-";
    document.getElementById("hours").innerText = currentTime.getHours();
    document.getElementById("minutes").innerText = currentTime.getMinutes();
    document.getElementById("seconds").innerText = currentTime.getSeconds();
  }
  else{
    document.getElementById("days").innerText = days;
    document.getElementById("hours").innerText = hours;
    document.getElementById("minutes").innerText = minutes;
    document.getElementById("seconds").innerText = seconds;
  }
}

setInterval(updateCountdown, 1000);