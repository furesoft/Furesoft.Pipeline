*ToDo*

[ ] Add Components
    class MyComponent : Component {
        public override string Render() {
            return "<h1>{{ atts.title }}</h1><p>{{ body }}</p>";
        }
    }

    <MyComponent title="hello world" />>
[ ] Add nestet components
    <mylist>
        <myitem>hello</myitem>
        <myitem>world</myitem>
    </mylist>