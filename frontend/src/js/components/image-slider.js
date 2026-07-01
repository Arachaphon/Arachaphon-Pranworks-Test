const imageSlider = () => ({
  currentIndex: 0,
  visibleSlides: 3,
  autoplayTimer: null,
  slides: [
    { src: 'src/images/grid-image/image-01.png', alt: 'image 1' },
    { src: 'src/images/grid-image/image-02.png', alt: 'image 2' },
    { src: 'src/images/grid-image/image-03.png', alt: 'image 3' },
    { src: 'src/images/grid-image/image-04.png', alt: 'image 4' },
    { src: 'src/images/grid-image/image-05.png', alt: 'image 5' },
    { src: 'src/images/grid-image/image-06.png', alt: 'image 6' },
    { src: 'src/images/product/product-01.jpg', alt: 'image 7' },
    { src: 'src/images/product/product-02.jpg', alt: 'image 8' },
    { src: 'src/images/product/product-03.jpg', alt: 'image 9' },
    { src: 'src/images/product/product-04.jpg', alt: 'image 10' },
    { src: 'src/images/product/product-05.jpg', alt: 'image 11' },
  ],

  get maxIndex() {
    return this.slides.length - this.visibleSlides;
  },

  get transformStyle() {
    const pct = this.currentIndex * (100 / this.visibleSlides);
    return `transform: translateX(-${pct}%); transition: transform 0.5s cubic-bezier(0.4, 0, 0.2, 1)`;
  },

  init() {
    this.startAutoplay();
  },

  startAutoplay() {
    this.stopAutoplay();
    this.autoplayTimer = setInterval(() => {
      if (this.currentIndex < this.maxIndex) {
        this.currentIndex++;
      } else {
        this.currentIndex = 0;
      }
    }, 5000);
  },

  stopAutoplay() {
    if (this.autoplayTimer) {
      clearInterval(this.autoplayTimer);
      this.autoplayTimer = null;
    }
  },

  next() {
    this.stopAutoplay();
    if (this.currentIndex < this.maxIndex) this.currentIndex++;
  },

  prev() {
    this.stopAutoplay();
    if (this.currentIndex > 0) this.currentIndex--;
  },

  goTo(index) {
    this.stopAutoplay();
    this.currentIndex = Math.min(index, this.maxIndex);
  },
});

export default imageSlider;
