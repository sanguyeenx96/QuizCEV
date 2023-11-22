# clock-tz.js
Clock widget/plugin with timezone options without jQuery. If you want show time in your timezone or show 

## Installation:

`npm i clock-tz`

## Usage

In html:

```html
<!-- Date & Time container you can styling anything -->
<div>
    <span id="date"></span>
    <span id="month"></span>
    <span id="year"></span>
    
    <span id="hours"></span>:<span id="minutes"></span>:<span id="seconds"></span>
</div>

<script src="clock.min.js"></script>
<script src="script.js"></script>
```

In `script.js`:

```javascript
// you can set your timezone or any other, or use Clock class without any options by default settings.
const options = {
    timeZoneOffset: +8.00
};
const clock = new Clock(options);
clock.start();
```


### Default settings

| Option name    | Default values                                                                                                             | Type   | Description                                            |
|----------------|----------------------------------------------------------------------------------------------------------------------------|--------|--------------------------------------------------------|
| dateId         | date                                                                                                                       | string | ID html element for day display                        |
| monthId        | month                                                                                                                      | string | ID html element for month display                      |
| yearId         | year                                                                                                                       | string | ID html element for year display                       |
| hoursId        | hours                                                                                                                      | string | ID html element for hours display                      |
| minutesId      | minutes                                                                                                                    | string | ID html element for minutes display                    |
| secondsId      | seconds                                                                                                                    | string | ID html element for seconds display                    |
| timeZoneOffset | null                                                                                                                       | float  | Time zone offset from UTC, example: +8.00 for UTC+8:00 |
| monthNames     | ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'] | Array  | Month names for display. From January to December.     |

## Contributing

1. Fork it
2. Create your feature branch (`git checkout -b my-new-feature`)
3. Commit your changes (`git commit -am 'Add some feature'`)
4. Push to the branch (`git push origin my-new-feature`)
5. Create new Pull Request
