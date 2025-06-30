import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
  scenarios: {
    ultra_spike: {
      executor: 'ramping-arrival-rate',
      startRate: 100,
      timeUnit: '1s',
      preAllocatedVUs: 500,
      maxVUs: 5000,
      stages: [
        { target: 500, duration: '10s' },
        { target: 5000, duration: '15s' },  // 🚀 spike peak
        { target: 5000, duration: '15s' },  // sustain high load
        { target: 300, duration: '10s' },
        { target: 0, duration: '10s' },
      ],
    },
  },
  thresholds: {
    http_req_duration: ['p(95)<1200'], // loosened threshold for extreme load
    checks: ['rate>0.95'],
  },
  insecureSkipTLSVerify: true,
};

const BASE_URL = 'https://localhost:8081/graphql';

const QUERY = `
  query {
    tenants {
      id
      companyName
      createdAt
    }
  }
`;

export default function () {
  const headers = { 'Content-Type': 'application/json' };
  const res = http.post(BASE_URL, JSON.stringify({ query: QUERY }), { headers });

  check(res, {
    'status is 200': (r) => r.status === 200,
    'resp < 1000ms': (r) => r.timings.duration < 1000,
  });

  sleep(Math.random() * 0.2); // let it breathe
}
