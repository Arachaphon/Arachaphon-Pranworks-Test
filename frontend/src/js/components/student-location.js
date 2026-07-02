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
  _destroyed: false,
  _initRetries: 0,

  init() {
    this._destroyed = false;
    this.getCurrentPosition();

    this.$el.addEventListener("alpine:destroy", () => {
      this._destroyed = true;
      this.destroyMap();
    });
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
        if (this._destroyed) return;
        this.lat = position.coords.latitude;
        this.lng = position.coords.longitude;
        this.accuracy = Math.round(position.coords.accuracy);
        this.loading = false;
        this._initRetries = 0;
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
    this.destroyMap();
    this.getCurrentPosition();
  },

  destroyMap() {
    if (this.map) {
      this.map.stop();
      this.map.off();
      this.map.remove();
      this.map = null;
      this.marker = null;
    }
  },

  initMap() {
    if (this._destroyed) return;
    if (this.map) return;

    const el = document.getElementById("studentMap");
    if (!el) return;

    // BUG เดิม: ถ้า container ยังไม่มีขนาด (สูง/กว้าง = 0 ตอนแรก render)
    // โค้ดจะ return ทิ้งเฉยๆ และไม่เคยสร้าง map เลย -> หน้าจอเป็นกล่องขาวว่าง
    // แก้ไข: ให้ retry ด้วย requestAnimationFrame จนกว่า container จะมีขนาดจริง
    if (el.clientWidth === 0 || el.clientHeight === 0) {
      if (this._initRetries < 50) { // กันลูปไม่รู้จบ (~50 เฟรม)
        this._initRetries++;
        requestAnimationFrame(() => this.initMap());
      }
      return;
    }

    if (this.lat == null || this.lng == null) return;

    this.map = L.map(el, {
      zoomControl: true,
      zoomAnimation: false,
      fadeAnimation: false,
      markerZoomAnimation: false,
    }).setView([this.lat, this.lng], 15);

    L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
      attribution:
        '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>',
      maxZoom: 19,
    }).addTo(this.map);

    this.marker = L.marker([this.lat, this.lng])
      .addTo(this.map)
      .bindPopup(
        `<b>คุณอยู่ที่นี่</b><br>ละติจูด: ${this.lat.toFixed(6)}<br>ลองจิจูด: ${this.lng.toFixed(6)}`,
      )
      .openPopup();

    L.circle([this.lat, this.lng], {
      radius: this.accuracy || 50,
      color: "#465fff",
      fillColor: "#465fff",
      fillOpacity: 0.15,
      weight: 2,
    }).addTo(this.map);

    // เผื่อ container เปลี่ยนขนาดหลัง render (เช่น animation ของ layout framework)
    requestAnimationFrame(() => {
      if (this.map) this.map.invalidateSize();
    });
    setTimeout(() => {
      if (this.map) this.map.invalidateSize();
    }, 300);
  },

  destroy() {
    this._destroyed = true;
    this.destroyMap();
  },
});

export default studentLocation;