const API_BASE = "http://localhost:5033";

export async function fetchMembers() {
  const res = await fetch(`${API_BASE}/api/members`);
  if (!res.ok) throw new Error(`HTTP ${res.status}`);
  return res.json();
}

export async function fetchProducts() {
  const res = await fetch(`${API_BASE}/api/products`);
  if (!res.ok) throw new Error(`HTTP ${res.status}`);
  return res.json();
}

export async function fetchOrders() {
  const res = await fetch(`${API_BASE}/api/orders`);
  if (!res.ok) throw new Error(`HTTP ${res.status}`);
  return res.json();
}

export async function searchProducts({ keyword, minPrice, maxPrice, isAvailable }) {
  const params = new URLSearchParams();
  if (keyword) params.set("keyword", keyword);
  if (minPrice != null) params.set("minPrice", minPrice);
  if (maxPrice != null) params.set("maxPrice", maxPrice);
  if (isAvailable != null) params.set("isAvailable", isAvailable);
  const res = await fetch(`${API_BASE}/api/products/search?${params}`);
  if (!res.ok) throw new Error(`HTTP ${res.status}`);
  return res.json();
}

export async function createMember(data) {
  const res = await fetch(`${API_BASE}/api/members`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
  if (!res.ok) throw new Error(`HTTP ${res.status}`);
  return res.json();
}

export async function createOrder(data) {
  const res = await fetch(`${API_BASE}/api/orders`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
  if (!res.ok) throw new Error(`HTTP ${res.status}`);
  return res.json();
}
