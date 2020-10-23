'use strict';



class LikeButton extends React.Component {

    constructor(props) {
        super(props);
        this.state = { liked: false };
    }

    render() {
        if (this.state.liked) {
            return 'Liked num' + this.props.commentID
        }

        return (
            <button onClick={() => this.setState({ liked:true })}>
                Like
            </button>
        );

    }
}

/*
$(".like_button_container").each(function () {
    const commentID = parseInt(this.dataset.commentid, 10);
    ReactDOM.render(
        React.createElement(LikeButton, { commentID: commentID }),
        this
    );
});*/


