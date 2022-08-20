import http from 'k6/http';
import { group, check, sleep } from "k6";
import { Counter } from 'k6/metrics';

// A simple counter for http requests

export const requests = new Counter('http_reqs');
export const jjcc = new Counter('jj_cc');


// you can specify stages of your test (ramp up/down patterns) through the options object
// target is the number of VUs you are aiming for

export const options = {
    stages: [
        { target: 20, duration: '10s' },
        { target: 20, duration: '10s' },
        { target: 0, duration: '10s' },
    ],
    thresholds: {
        jj_cc : ['count < 1'],
    },
};

const BASE_URL = "https://localhost:5050";
const SLEEP_DURATION = 0.1;

export default function () {
    group("/api/Values", () => {

        // Request No. 1
        {
            let url = BASE_URL + `/api/Values`;
            let request = http.get(url);

            let resGet =check(request, {
                "Success": (r) => r.status === 200
            });
            if (!resGet)
                jjcc.add(1);
            sleep(SLEEP_DURATION);
        }

        // Request No. 2
        {
            let url = BASE_URL + `/api/Values`;
            let params = { headers: { "Content-Type": "application/json", "Accept": "application/json" } };
            let payload = JSON.stringify(
                {
                    "Message": "123 ",
                    "ReceiveDateTime": "2022-08-20T10:40:15"
                }

            );
            let request = http.post(url, payload, params);

           let resPost= check(request, {
                "Success": (r) => r.status === 200
           });

           if (!resPost)
               jjcc.add(1);
        }
      
    });

}
