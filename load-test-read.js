import http from 'k6/http';
import { check } from 'k6';

export const options = {
  scenarios: {
    read_test: {
      executor: 'constant-arrival-rate',
      rate: 1000, // 🔥 1000 RPS
      timeUnit: '1s',
      duration: '10m',
      preAllocatedVUs: 200,
      maxVUs: 1500,
    },
  },
  thresholds: {
    http_req_duration: ['p(95)<200'],
    http_req_failed: ['rate<0.01'],
  },
};

const BASE_URL = 'http://localhost:8080';

const searchTerms = ['acme', 'corp', 'globex', 'tech', 'inc', 'group', 'solutions'];

export default function () {
  const page = Math.floor(Math.random() * 50) + 1;
  const pageSize = [5, 10, 20, 50][Math.floor(Math.random() * 4)];
  const search = searchTerms[Math.floor(Math.random() * searchTerms.length)];

  const url = `${BASE_URL}/api/tenants?page=${page}&pageSize=${pageSize}&search=${search}`;

  const res = http.get(url);

  check(res, {
    'status is 200': (r) => r.status === 200,
  });
}
