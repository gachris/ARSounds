var html = "\
<div class='m-audio-content'>\
    <div class='play-pause' style='display: grid; cursor: pointer; '>\
        <div class='btn btn-play play-pause-content'></div>\
    </div>\
    <div class='playControls__repeat playControls__control'>\
        <button title ='Repeat' class='btn repeatControl sc-ir m-none'></button>\
    </div>\
    <div style='margin: auto 1.2rem;'>\
        <small class='audio-passedtime process-text'>0:00</small>\
    </div>\
    <div class='progress-audio-container'>\
    <div class='progress-audio'>\
        <input class='progress-audio-range' type='range' min='0' max='100' value='0' id='fader' step='0.1'>\
    </div>\
    </div>\
    <div style='margin: auto 1.2rem;'>\
        <small class='audio-durationtime text'>0:00</small>\
    </div>\
    <div class='playControls__volume'>\
        <div class='volume' data-level='10'>\
            <div class='volume__iconWrapper'>\
                <button type='button' class='volume__button volume__speakerIcon sc-ir'>\
                </button>\
            </div>\
            <div class='volume__sliderWrapper'>\
                <input type='range' orient=vertical class='volume__input' value='10' min='0' max='10' step='1' />\
            </div>\
        </div>\
    </div>\
</div>";

$(".m-audio").initialize(function () {
  var audio = $(this)[0];

  $(audio).after(html);

  var parrent = $(audio).parent();
  var play_pause = $(parrent).find('.play-pause');
  var play_pause_content = $(parrent).find('.play-pause-content');
  var progress_bar = $(parrent).find('.progress-audio-range');
  var audio_passedtime = $(parrent).find('.audio-passedtime');
  var audio_durationtime = $(parrent).find('.audio-durationtime');

  var volume = parrent.find('.volume');
  var volume__input = volume.find('.volume__input');
  var volume__button = parrent.find('.volume__button');

  var repeatControl = parrent.find('.repeatControl');

  $(play_pause).on('click', function () {
    if (audio.paused) {
      audio.play();
    }
    else {
      audio.pause();
    }
  })

  audio.addEventListener("durationchange", function (e) {
    if (this.duration != Infinity) {
      setdurationtime(audio_durationtime, audio);
    }
  }, false);

  setdurationtime(audio_durationtime, audio);

  $(audio).on("play", function () {
    $(play_pause_content).removeClass("btn-play");
    $(play_pause_content).addClass("btn-pause");
  });

  $(audio).on("pause", function () {
    $(play_pause_content).removeClass("btn-pause");
    $(play_pause_content).addClass("btn-play");
  });

  $(audio).on("timeupdate", function () {
    setpassedtime(audio_passedtime, audio);
    updateprogressbar(progress_bar, audio);
    SetProcessColor(progress_bar);
  });

  $(audio).on("ended", function () {
    $(play_pause_content).removeClass("btn-pause");
    $(play_pause_content).addClass("btn-play");
    $(progress_bar).value = 0;
    $(audio_passedtime).text('0:00');
  });

  $(audio).on("volumechange", function () {
    if (audio.muted) {
      $(volume).attr("data-level", "0");
    }
    else {
      var value = audio.volume * 10;
      $(volume).attr("data-level", value);
    }
  });

  $(progress_bar).on('input', function (e) {
    seek(audio, progress_bar);
  })

  $(volume__button).on('click', function () {
    if (audio.muted) {
      audio.muted = false;
    }
    else {
      audio.muted = true;
    }
  })

  $(volume).on('mouseover', function () {
    $(volume).addClass("expanded");
  })

  $(volume).on('mouseout', function () {
    $(volume).removeClass("expanded");
  })

  $(volume__input).on('input', function () {
    audio.volume = volume__input.val() / 10;
  })

  $(repeatControl).on('click', function (e) {
    if ($(audio).attr("loop") === 'loop') {
      $(audio).removeAttr("loop");
      $(repeatControl).removeClass("m-one");
      $(repeatControl).addClass("m-none");
    } else {
      $(audio).attr("loop", "loop");
      $(repeatControl).removeClass("m-none");
      $(repeatControl).addClass("m-one");
    }
  })

  $(progress_bar).on('input', function () {
    SetProcessColor($(progress_bar));
  });
});

function SetProcessColor(progress_bar) {
  var val = (progress_bar.val() - progress_bar.attr('min')) / (progress_bar.attr('max') - progress_bar.attr('min'));

  progress_bar.css('background-image',
    '-webkit-gradient(linear, left top, right top, '
    + 'color-stop(' + val + ', #FF5500), '
    + 'color-stop(' + val + ', #C5C5C5)'
    + ')'
  );
}

function setdurationtime(text_element, audio) {
  var minutes = parseInt(audio.duration / 60, 10);
  var seconds = parseInt(audio.duration % 60);

  if (seconds < 10) {
    seconds = '0' + String(seconds);
  }

  $(text_element).text(minutes + ':' + seconds);
}

function setpassedtime(text_element, audio) {
  var mins = Math.floor(audio.currentTime / 60);
  if (mins < 10) {
    mins = String(mins);
  }

  var secs = Math.floor(audio.currentTime % 60);
  if (secs < 10) {
    secs = '0' + String(secs);
  }

  $(text_element).text(mins + ':' + secs);
}

function updateprogressbar(progress_bar, audio) {
  var currentTime = audio.currentTime;
  var duration = audio.duration;
  var newprogress = currentTime / duration * 100;
  $(progress_bar).val(newprogress);
}

function seek(audio, progress) {
  var value = audio.duration * (progress.val() / 100);
  audio.currentTime = value;
}
