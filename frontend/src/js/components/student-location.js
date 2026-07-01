import L from "leaflet";
import "leaflet/dist/leaflet.css";
import markerIcon2x from "leaflet/dist/images/marker-icon-2x.png";
import markerIcon from "leaflet/dist/images/marker-icon.png";
import markerShadow from "leaflet/dist/images/marker-shadow.png";

delete L.Icon.Default.prototype._getIconUrl;
L.Icon.Default.mergeOptions({
  iconRetinaUrl: markerIcon2x,
  iconUrl: markerIcon,
  shadowUrl: markerShadow,
});

const studentLocation = () => ({
  loading: true,
  error: null,
  lat: null,
  lng: null,
  accuracy: null,
  map: null,
  marker: null,

  init() {
    this.getCurrentPosition();
  },

  getCurrentPosition() {
    if (!navigator.geolocation) {
      this.error = "เบราว์เซอร์ของคุณไม่รองรับ Geolocation API";
      this.loading = false;
      return;
    }

    this.loading = true;
    this.error = null;

    navigator.geolocation.getCurrentPosition(
      (position) => {
        this.lat = position.coords.latitude.toFixed(6);
        this.lng = position.coords.longitude.toFixed(6);
        this.accuracy = Math.round(position.coords.accuracy);
        this.loading = false;
        this.$nextTick(() => this.initMap());
      },
      (err) => {
        const messages = {
          1: "ไม่อนุญาตให้เข้าถึงตำแหน่ง (Permission Denied)",
          2: "ไม่สามารถระบุตำแหน่งได้ (Position Unavailable)",
          3: "การร้องขอตำแหน่งหมดเวลา (Timeout)",
        };
        this.error = messages[err.code] || `เกิดข้อผิดพลาด: ${err.message}`;
        this.loading = false;
      },
      {
        enableHighAccuracy: true,
        timeout: 15000,
        maximumAge: 0,
      },
    );
  },

  refreshLocation() {
    if (this.map) {
      this.map.remove();
      this.map = null;
      this.marker = null;
    }
    this.getCurrentPosition();
  },

  initMap() {
    this.map = L.map("studentMap", {
      zoomControl: true,
    }).setView([this.lat, this.lng], 15);

    L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
      attribution:
        '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>',
      maxZoom: 19,
    }).addTo(this.map);

    this.marker = L.marker([this.lat, this.lng])
      .addTo(this.map)
      .bindPopup(
        `<b>คุณอยู่ที่นี่</b><br>ละติจูด: ${this.lat}<br>ลองจิจูด: ${this.lng}`,
      )
      .openPopup();

    L.circle([this.lat, this.lng], {
      radius: this.accuracy || 50,
      color: "#465fff",
      fillColor: "#465fff",
      fillOpacity: 0.15,
      weight: 2,
    }).addTo(this.map);
  },
});

export default studentLocation;
