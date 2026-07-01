import { fetchMembers, fetchProducts, fetchOrders } from "../api";

export default function apiIntegration() {
  return {
    members: [],
    products: [],
    orders: [],
    loading: false,
    error: null,

    async loadAll() {
      this.loading = true;
      this.error = null;
      try {
        const [members, products, orders] = await Promise.all([
          fetchMembers(),
          fetchProducts(),
          fetchOrders(),
        ]);
        this.members = members;
        this.products = products;
        this.orders = orders;
      } catch (err) {
        this.error = err.message;
      } finally {
        this.loading = false;
      }
    },
  };
}
