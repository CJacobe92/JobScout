import http from 'k6/http';
import { check } from 'k6';

export const options = {
  scenarios: {
    read_test: {
      executor: 'constant-arrival-rate',
      rate: 1000,
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

const searchTerms = [
  'Acme', 'Morgan', 'Troy', 'Velocity', 'Orbit', 'Summit', 'Pulse', 'Nexus',
  'Fusion', 'Cascade', 'Zenith', 'Nova', 'Quantum'
];

const searchFields = ['name', 'registeredto', 'license'];

export default function () {
  const page = Math.floor(Math.random() * 50) + 1;
  const pageSize = [5, 10, 20, 50][Math.floor(Math.random() * 4)];
  const search = searchTerms[Math.floor(Math.random() * searchTerms.length)];
  const by = searchFields[Math.floor(Math.random() * searchFields.length)];

  const url = `${BASE_URL}/api/tenants?search=${encodeURIComponent(search)}&by=${by}&page=${page}&pageSize=${pageSize}`;

  const res = http.get(url);

  check(res, {
    'status is 200': (r) => r.status === 200,
    'response is not empty': (r) => r.body && r.body.length > 2,
  });
}
