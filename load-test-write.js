import http from 'k6/http';
import { check } from 'k6';

export const options = {
  scenarios: {
    soak_test: {
      executor: 'constant-arrival-rate',
      rate: 1000, // 🔥 Sustained 1000 RPS
      timeUnit: '1s',
      duration: '10m', // ⏱️ Soak for 10 minutes
      preAllocatedVUs: 300,
      maxVUs: 1500,
    },
  },
  thresholds: {
    http_req_duration: ['p(95)<500'], // ✅ Ensure latency stays under 500ms
    http_req_failed: ['rate<0.01'],   // ✅ Ensure failure rate is minimal
  },
};

const BASE_URL = 'http://localhost:8080';

function randomString(length) {
  const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
  return Array.from({ length }, () => chars[Math.floor(Math.random() * chars.length)]).join('');
}

function randomCompanyName() {
  const prefixes = ['Acme', 'Globex', 'Initech', 'Umbrella', 'Wayne', 'Stark', 'Hooli'];
  const suffixes = ['Corp', 'Inc', 'LLC', 'Ltd', 'Group'];
  return `${prefixes[Math.floor(Math.random() * prefixes.length)]} ${suffixes[Math.floor(Math.random() * suffixes.length)]}`;
}

function randomPersonName() {
  const firstNames = ['John', 'Jane', 'Alex', 'Chris', 'Taylor', 'Jordan', 'Morgan'];
  const lastNames = ['Doe', 'Smith', 'Johnson', 'Lee', 'Brown', 'Garcia', 'Martinez'];
  return `${firstNames[Math.floor(Math.random() * firstNames.length)]} ${lastNames[Math.floor(Math.random() * lastNames.length)]}`;
}

export default function () {
  const payload = JSON.stringify({
    name: randomCompanyName(),
    license: `LIC-${new Date().getFullYear()}-${randomString(6).toUpperCase()}`,
    phone: '+639171234567',
    registeredTo: randomPersonName(),
    tin: `${Math.floor(100 + Math.random() * 900)}-${Math.floor(100 + Math.random() * 900)}-${Math.floor(100 + Math.random() * 900)}`,
    address: '123 Makati Ave, Metro Manila'
  });

  const headers = { 'Content-Type': 'application/json' };

  const res = http.post(`${BASE_URL}/api/tenants`, payload, { headers });

  check(res, {
    'status is 201': (r) => r.status === 201,
  });
}
