/** @type {import('tailwindcss').Config} */
export default {
  purge: ['./index.html', './src/**/*.{vue,js,ts,jsx,tsx}'],
  content: [],
  theme: {
    extend: {
      backgroundImage: {
        'home-banner': "url('https://www.greatschools.org/gk/wp-content/uploads/2010/01/Looking-for-a-tutor.jpg')",
        'contact-banner' : "url('https://publicassets.teachme2.com/www/images/become-a-tutor/female-blonde-tutor-tutoring-young-girl.jpg')"
      }
    },
  },
  plugins: [],
}

