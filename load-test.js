import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    scenarios: {
        crazy_graphql_firestorm: {
            executor: 'constant-arrival-rate',
            rate: 10000, // ⚠️ 10,000 requests per second!
            timeUnit: '1s',
            duration: '1m',
            preAllocatedVUs: 2000,
            maxVUs: 12000,
        },
    },
    thresholds: {
        http_req_duration: ['p(95)<1000'],
        checks: ['rate>0.98'],
    },
    insecureSkipTLSVerify: true,
};

const BASE_URL = 'https://localhost:8081/graphql';
const QUERY = `
  query {
    tenants {
      id
      companyName
    }
  }
`;

export default function () {
    const headers = { 'Content-Type': 'application/json' };
    const res = http.post(BASE_URL, JSON.stringify({ query: QUERY }), { headers });

    check(res, {
        'status is 200': (r) => r.status === 200,
        'response time < 800ms': (r) => r.timings.duration < 800,
    });

    sleep(Math.random() * 1); // light pacing
}
